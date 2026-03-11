interface SendData {
    action: string;
    target: string;
    value: string;
}

const target = "#theparagraph";

function sendMessage(data: SendData): void {
    chrome.tabs.query({active: true, currentWindow: true}, async (tabs) => {
        const tab = tabs[0];

        if (tab === undefined || tab.id === undefined) {
            throw new Error("Tab not found");
        }

        return await chrome.tabs.sendMessage(tab.id, data);
    });
}

function changeText(): void {
    sendMessage({action: "changeText", target: target, value: "Hello from Blazor!"});
}

function changeTextColor(): void {
    sendMessage({action: "changeTextColor", target: target, value: '#' + (Math.random() * 0xFFFFFF << 0).toString(16).padStart(6, '0')});
}

function changeTextSize(): void {
    sendMessage({action: "changeTextSize", target: target, value: (Math.random() * 100).toString() + 'pt'});
}
