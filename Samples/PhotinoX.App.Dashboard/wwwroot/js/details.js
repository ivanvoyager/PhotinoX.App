(function () {
    const elements = {
        appName: document.getElementById("details-app-name"),
        environment: document.getElementById("details-environment"),
        contentRoot: document.getElementById("details-content-root"),
        webRoot: document.getElementById("details-web-root"),

        time: document.getElementById("details-time"),
        uptime: document.getElementById("details-uptime"),
        managedMemory: document.getElementById("details-managed-memory"),
        nativeMemory: document.getElementById("details-native-memory"),
        privateMemory: document.getElementById("details-private-memory"),
        cpu: document.getElementById("details-cpu"),
        threads: document.getElementById("details-threads"),
        gc: document.getElementById("details-gc"),

        raw: document.getElementById("rawshot"),
        closeButton: document.getElementById("close-button")
    };

    function setText(element, value) {
        element.textContent = value || "-";
        element.title = value || "";
    }

    function renderSnapshot(snapshot) {
        setText(elements.appName, snapshot.application.name);
        setText(elements.environment, snapshot.application.environmentName);
        setText(elements.contentRoot, snapshot.configuration.contentRootPath);
        setText(elements.webRoot, snapshot.configuration.webRootPath);

        setText(elements.time, snapshot.runtime.currentTime);
        setText(elements.uptime, snapshot.runtime.uptime);
        setText(elements.managedMemory, snapshot.runtime.managedMemory);
        setText(elements.nativeMemory, snapshot.runtime.nativeMemory);
        setText(elements.privateMemory, snapshot.runtime.privateMemory);
        setText(elements.cpu, snapshot.runtime.cpuUsage);
        setText(
            elements.threads,
            `Worker: ${snapshot.runtime.threadPoolWorkers}, Process: ${snapshot.runtime.processThreadCount}`
        );
        setText(
            elements.gc,
            `Gen0: ${snapshot.runtime.gcGen0}, Gen1: ${snapshot.runtime.gcGen1}, Gen2: ${snapshot.runtime.gcGen2}`
        );

        elements.raw.textContent = JSON.stringify(snapshot, null, 2);
    }

    function handleHostMessage(message) {
        if (message.type === "snapshot") {
            renderSnapshot(message.data);
            return;
        }

        if (message.type === "error") {
            console.error(message.error);
        }
    }

    elements.closeButton.addEventListener("click", function () {
        window.photinoHost.closeWindow();
    });

    window.photinoHost.subscribe(handleHostMessage);

    function refresh() {
        window.photinoHost.getSnapshot();
    }

    refresh();
    setInterval(refresh, 1000);
})();