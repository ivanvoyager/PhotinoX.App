(function () {
    const handlers = new Set();

    function post(message) {
        const payload = JSON.stringify(message);

        if (window.external && typeof window.external.sendMessage === "function") {
            window.external.sendMessage(payload);
            return true;
        }

        console.warn("Photino host bridge is not available.");
        return false;
    }

    function subscribe(handler) {
        handlers.add(handler);

        return function unsubscribe() {
            handlers.delete(handler);
        };
    }

    function dispatch(message) {
        for (const handler of handlers) {
            handler(message);
        }
    }

    if (window.external && typeof window.external.receiveMessage === "function") {
        window.external.receiveMessage(function (message) {
            try {
                dispatch(JSON.parse(message));
            } catch (error) {
                console.error("Invalid host message.", error, message);
            }
        });
    }

    window.photinoHost = {
        post,
        subscribe,
        getSnapshot() {
            return post({ type: "getSnapshot" });
        },
        closeWindow() {
            return post({ type: "closeWindow" });
        }
    };
})();