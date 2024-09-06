(function (videojs) {
    const AudioContext = window.AudioContext || window.webkitAudioContext;
    if (!AudioContext) {
        console.error("AudioContext is not supported in this browser.");
        return;
    }

    const defaults = {
        waveColor: "#fff",
        waveWidth: null,
        waveHeight: 30,
        barWidth: 2,
        barSpacing: 1,
        fftSize: 256,
        visualizationType: "bars"
    };

    const Plugin = videojs.getPlugin("plugin");

    class SoundWavePlugin extends Plugin {
        constructor(player, options) {
            super(player);

            this.options = videojs.mergeOptions(defaults, options);

            this.player = player;
            this.audioContext = new AudioContext();
            this.analyser = this.audioContext.createAnalyser();
            this.dataArray = new Uint8Array(this.analyser.frequencyBinCount);

            this.canvas = document.createElement("canvas");
            this.canvasContext = this.canvas.getContext("2d");

            this.player.ready(() => {
                this.onPlayerReady();
            });
        }

        onPlayerReady() {
            this.canvas.width = (this.options.waveWidth || this.player.el().offsetWidth);
            this.canvas.height = this.options.waveHeight;
            this.canvas.style.position = "absolute";
            this.canvas.style.bottom = "0";
            this.canvas.style.left = "0";
            this.canvas.style.zIndex = "0";

            const bigPlayButton = this.player.el().querySelector(".vjs-big-play-button");

            if (bigPlayButton) {
                this.player.el().insertBefore(this.canvas, bigPlayButton);
            } else {
                this.player.el().appendChild(this.canvas);
            }
            
            const source = this.audioContext.createMediaElementSource(this.player.tech().el());
            source.connect(this.analyser);
            this.analyser.connect(this.audioContext.destination);
            
            this.analyser.fftSize = this.options.fftSize;
            
            this.player.on("play", () => {
                this.startVisualization();
            });

            this.player.on("pause", () => {
                this.stopVisualization();
            });
        }

        startVisualization() {
            if (!this.animationFrame) {
                const draw = () => {
                    this.animationFrame = requestAnimationFrame(draw);
                    this.drawVisualization();
                };
                draw();
            }
        }

        stopVisualization() {
            if (this.animationFrame) {
                cancelAnimationFrame(this.animationFrame);
                this.animationFrame = null;
            }
        }

        drawVisualization() {
            switch (this.options.visualizationType) {
                case "circular":
                    this.drawCircularVisualizer();
                    break;
                //case "particles":
                //    this.drawParticleSystem();
                //    break;
                case "lineGraph":
                    this.drawLineGraph();
                    break;
                case "waveform":
                    this.drawWaveform();
                    break;
                case "frequencySpectrum":
                    this.drawFrequencySpectrum();
                    break;
                default:
                    this.drawBars();
            }
        }

        drawBars() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteFrequencyData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.fillStyle = this.options.waveColor;

            const barCount = Math.floor(this.dataArray.length / 2); //Math.floor(width / (this.options.barWidth + this.options.barSpacing));
            const barWidth = (this.options.barWidth / 2);
            const barSpacing = this.options.barSpacing;

            const centerY = height / 2;
            const centerX = width / 2;

            for (let i = 0; i < barCount; i++) {
                const percent = this.dataArray[i] / 255;
                const barHeight = (height / 2) * percent;

                const rightX = centerX + i * (barWidth + barSpacing);
                context.fillRect(rightX, centerY - barHeight, barWidth, barHeight);
                context.fillRect(rightX, centerY, barWidth, barHeight);

                const leftX = centerX - (i + 1) * (barWidth + barSpacing);
                context.fillRect(leftX, centerY - barHeight, barWidth, barHeight);
                context.fillRect(leftX, centerY, barWidth, barHeight);
            }
        }

        drawCircularVisualizer() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteFrequencyData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.strokeStyle = this.options.waveColor;

            const centerX = width / 2;
            const centerY = height / 2;
            const radius = Math.min(centerX, centerY) - 5;

            context.beginPath();
            for (let i = 0; i < this.dataArray.length; i++) {
                const percent = this.dataArray[i] / 255;
                const radian = (i / this.dataArray.length) * Math.PI * 2;
                const barHeight = radius * percent;

                const x = centerX + Math.cos(radian) * (radius - barHeight);
                const y = centerY + Math.sin(radian) * (radius - barHeight);

                if (i === 0) {
                    context.moveTo(x, y);
                } else {
                    context.lineTo(x, y);
                }
            }
            context.closePath();
            context.stroke();
        }

        drawLineGraph() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteFrequencyData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.strokeStyle = this.options.waveColor;
            context.lineWidth = 2;

            context.beginPath();
            for (let i = 0; i < this.dataArray.length; i++) {
                const x = (i / this.dataArray.length) * width;
                const y = height - (this.dataArray[i] / 255) * height;

                if (i === 0) {
                    context.moveTo(x, y);
                } else {
                    context.lineTo(x, y);
                }
            }
            context.stroke();
        }

        drawWaveform() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteTimeDomainData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.strokeStyle = this.options.waveColor;
            context.lineWidth = 2;

            context.beginPath();
            for (let i = 0; i < this.dataArray.length; i++) {
                const x = (i / this.dataArray.length) * width;
                const y = (this.dataArray[i] / 255) * height;

                if (i === 0) {
                    context.moveTo(x, y);
                } else {
                    context.lineTo(x, y);
                }
            }
            context.stroke();
        }

        drawFrequencySpectrum() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteFrequencyData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.fillStyle = this.options.waveColor;

            const barWidth = width / this.dataArray.length;

            for (let i = 0; i < this.dataArray.length; i++) {
                const barHeight = (this.dataArray[i] / 255) * height;
                const x = i * barWidth;
                const y = height - barHeight;

                context.fillRect(x, y, barWidth, barHeight);
            }
        }
    }

    videojs.registerPlugin("soundWave", SoundWavePlugin);
})(window.videojs);