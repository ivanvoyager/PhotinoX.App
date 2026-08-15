(function () {
    const elements = {
        status: document.getElementById("app-status"),

        appName: document.getElementById("app-name"),
        environmentName: document.getElementById("environment-name"),
        initializedAt: document.getElementById("initialized-at"),

        nativeVersion: document.getElementById("native-version"),
        webViewEngine: document.getElementById("webview-engine"),
        webViewRuntime: document.getElementById("webview-runtime"),
        platformRuntimeDetails: document.getElementById("platform-runtime-details"),
        processArchitecture: document.getElementById("process-architecture"),

        currentTime: document.getElementById("current-time"),
        uptime: document.getElementById("uptime"),
        managedMemory: document.getElementById("managed-memory"),
        nativeMemory: document.getElementById("native-memory"),
        cpuUsage: document.getElementById("cpu-usage"),

        contentRoot: document.getElementById("content-root"),
        webRoot: document.getElementById("web-root"),
        mainWindowConfig: document.getElementById("main-window-config"),
        detailsWindowConfig: document.getElementById("details-window-config"),

        windowsList: document.getElementById("windows-list")
    };

    function setText(element, value) {
        element.textContent = value || "-";
        element.title = value || "";
    }

    function renderSnapshot(snapshot) {
        setText(elements.status, snapshot.application.status);

        setText(elements.appName, snapshot.application.name);
        setText(elements.environmentName, snapshot.application.environmentName);
        setText(elements.initializedAt, snapshot.application.initializedAt);

        const photino = snapshot.photinoRuntime;

        setText(elements.nativeVersion, photino.nativeVersion);
        setText(elements.webViewEngine, photino.webViewEngine);
        setText(elements.webViewRuntime, photino.webViewRuntimeVersion);
        setText(elements.processArchitecture, photino.processArchitecture);

        renderPlatformDetails(photino.platformDetails);

        setText(elements.currentTime, snapshot.runtime.currentTime);
        setText(elements.uptime, snapshot.runtime.uptime);
        setText(elements.managedMemory, snapshot.runtime.managedMemory);
        setText(elements.nativeMemory, snapshot.runtime.nativeMemory);
        setText(elements.cpuUsage, snapshot.runtime.cpuUsage);

        setText(elements.contentRoot, snapshot.configuration.contentRootPath);
        setText(elements.webRoot, snapshot.configuration.webRootPath);
        setText(elements.mainWindowConfig, snapshot.configuration.mainWindow);
        setText(elements.detailsWindowConfig, snapshot.configuration.detailsWindow);

        renderWindows(snapshot.windows);
    }

    function renderPlatformDetails(items) {
        if (!items || items.length === 0) {
            elements.platformRuntimeDetails.innerHTML = "";
            return;
        }

        elements.platformRuntimeDetails.innerHTML = items
            .map(item => `
          <div>
            <dt>${escapeHtml(item.name)}</dt>
            <dd title="${escapeHtml(item.value)}">${escapeHtml(item.value)}</dd>
          </div>
        `)
            .join("");
    }

    function renderWindows(windows) {
        if (!windows || windows.length === 0) {
            elements.windowsList.innerHTML = '<p class="muted">No windows reported.</p>';
            return;
        }

        elements.windowsList.innerHTML = windows
            .map(windowInfo => {
                const badgeClass = windowInfo.status === "Open" ? "window-badge open" : "window-badge";

                return `
          <div class="window-row">
            <div>
              <strong>${escapeHtml(windowInfo.name)}</strong>
              <span>${escapeHtml(windowInfo.title)}</span>
            </div>
            <span class="${badgeClass}">${escapeHtml(windowInfo.status)}</span>
          </div>
        `;
            })
            .join("");
    }

    function escapeHtml(value) {
        return String(value ?? "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    function handleHostMessage(message) {
        if (message.type === "snapshot") {
            renderSnapshot(message.data);
            return;
        }

        if (message.type === "error") {
            setText(elements.status, "Host error");
            console.error(message.error);
        }
    }

    window.photinoHost.subscribe(handleHostMessage);

    function refresh() {
        window.photinoHost.getSnapshot();
    }

    refresh();
    setInterval(refresh, 1000);
})();