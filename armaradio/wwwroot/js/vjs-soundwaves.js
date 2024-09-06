(function (videojs) {
    const AudioContext = window.AudioContext || window.webkitAudioContext;
    if (!AudioContext) {
        console.error('AudioContext is not supported in this browser.');
        return;
    }

    const defaults = {
        waveColor: '#fff',
        waveHeight: 30,
        barWidth: 2,
        barSpacing: 1,
        fftSize: 256
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
            this.canvas.width = this.player.el().offsetWidth;
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
                    this.drawWave();
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

        drawWave() {
            const width = this.canvas.width;
            const height = this.canvas.height;
            const context = this.canvasContext;

            this.analyser.getByteFrequencyData(this.dataArray);

            context.clearRect(0, 0, width, height);
            context.fillStyle = this.options.waveColor;

            const barCount = Math.floor(width / (this.options.barWidth + this.options.barSpacing));
            const barWidth = this.options.barWidth;
            const barSpacing = this.options.barSpacing;

            const centerY = height / 2;

            for (let i = 0; i < barCount; i++) {
                const percent = this.dataArray[i] / 255;
                const barHeight = (height / 2) * percent;
                const x = i * (barWidth + barSpacing);

                context.fillRect(x, centerY - barHeight, barWidth, barHeight);
                context.fillRect(x, centerY, barWidth, barHeight);
            }
        }
    }

    videojs.registerPlugin("soundWave", SoundWavePlugin);
})(window.videojs);