
scrollToTop = function () {
    window.scrollTo({ top:0,   behavior: "smooth" });
}

getIsDarkMode = function () {
    let result = window.localStorage.getItem("isDarkModeEnabled");
    if (result == "true")
        return true;
    else
        return false;
}

setIsDarkMode = function (value) {
    window.localStorage.setItem("isDarkModeEnabled", value)
}
