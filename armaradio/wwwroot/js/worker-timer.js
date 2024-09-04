/**
 * 
 * EXAMPLES:
 *      workerTimeout(function (value = "dsadfa") { console.log(value)}, 1000);
 *      workerTimeout(callMyFunctionByName, 1000);
 * 
 *      let currentWorker = workerInterval(function (value = "dsadfa") { console.log(value)}, 1000);
 *      workerClearInterval(currentWorker);
 * 
 */

function workerTimeout(callback, timeoutTime) {
    let wwTimerHandler = `setTimeout(function() {
        self.postMessage({});
    }, ` + (timeoutTime || 250) + `);`;

    let worker = new Worker(
        window.URL.createObjectURL(
            new Blob([`self.onmessage=${wwTimerHandler}`])
        )
    );

    worker.onmessage = function (data) {
        if (callback) {
            let wwAllCallbacks = $.Callbacks();
            wwAllCallbacks.add(callback);
            wwAllCallbacks.fire();
        }
    };
}

function workerInterval(callback, timeoutTime) {
    let wwTimerHandler = `setInterval(function() {
        self.postMessage({});
    }, ` + (timeoutTime || 250) + `);`;

    let worker = new Worker(
        window.URL.createObjectURL(
            new Blob([`self.onmessage=${wwTimerHandler}`])
        )
    );

    worker.onmessage = function (data) {
        if (callback) {
            let wwAllCallbacks = $.Callbacks();
            wwAllCallbacks.add(callback);
            wwAllCallbacks.fire();
        }
    };

    return worker;
}

function wokerClearInterval(worker) {
    if (worker) {
        worker.terminate();
    }
}