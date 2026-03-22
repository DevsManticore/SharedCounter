mergeInto(LibraryManager.library, {

    SavePlayerName: function (ptr) {
        var name = UTF8ToString(ptr);
        localStorage.setItem("playerName", name);
    },

    LoadPlayerName: function () {
        var name = localStorage.getItem("playerName");
        if (!name) return 0;

        var size = lengthBytesUTF8(name) + 1;
        var buffer = _malloc(size);
        stringToUTF8(name, buffer, size);
        return buffer;
    },

    ShowAlert: function (ptr) {
        var msg = UTF8ToString(ptr);
        alert(msg);
    },

    GetURLParam: function (ptr) {
        var param = UTF8ToString(ptr);
        var params = new URLSearchParams(window.location.search);
        var value = params.get(param);

        if (!value) return 0;

        var size = lengthBytesUTF8(value) + 1;
        var buffer = _malloc(size);
        stringToUTF8(value, buffer, size);
        return buffer;
    },

    CopyToClipboard: function (ptr) {
        var text = UTF8ToString(ptr);

        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text);
        } else {
            var textarea = document.createElement("textarea");
            textarea.value = text;
            document.body.appendChild(textarea);
            textarea.select();

            try {
                document.execCommand("copy");
            } catch (err) {
                console.error("Clipboard fallback failed", err);
            }

            document.body.removeChild(textarea);
        }
    }
});