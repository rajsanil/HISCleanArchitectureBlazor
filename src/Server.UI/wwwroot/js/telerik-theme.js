function changeTelerikTheme(newUrl) {
    var oldLink = document.getElementById("telerik-theme");

    if (!oldLink) {
        console.error("Telerik theme link not found");
        return;
    }

    if (newUrl === oldLink.getAttribute("href")) {
        return;
    }

    var newLink = document.createElement("link");
    newLink.setAttribute("id", "telerik-theme");
    newLink.setAttribute("rel", "stylesheet");
    newLink.setAttribute("href", newUrl);
    newLink.onload = () => {
        oldLink.parentElement.removeChild(oldLink);
    };

    document.getElementsByTagName("head")[0].appendChild(newLink);
}
