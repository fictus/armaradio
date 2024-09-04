self.onmessage = function (event) {
    const canvas = event.data.canvas;
    const ctx = canvas.getContext('2d');

    // Set the canvas dimensions
    const width = canvas.width;
    const height = canvas.height;

    // A simple animation loop
    let x = 0;
    const speed = 2;

    function draw() {
        // Clear the canvas
        ctx.clearRect(0, 0, width, height);

        // Draw a rectangle
        ctx.fillStyle = 'blue';
        ctx.fillRect(x, height / 2 - 25, 50, 50);

        // Update the rectangle's position
        x += speed;
        if (x > width) {
            x = 0;
        }

        // Request the next frame
        requestAnimationFrame(draw);
    }

    // Start the animation
    draw();
};