interface SendData {
    action: string;
    target: string;
    value: string;
}

chrome.runtime.onMessage.addListener((request: SendData, sender, sendResponse) => {
    const target = document.querySelector(request.target) as HTMLElement;

    if (target === null || target === undefined) {
        throw new Error("target not found");
    }
    
    switch (request.action) {
        case "changeText":
            target.textContent = request.value;
            break;

        case "changeTextColor":
            target.style.color = request.value;
            break;

        case "changeTextSize":
            target.style.fontSize = request.value;
            break;
    }

    sendResponse({status: 'success'});
});
