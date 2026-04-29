(function (root, factory) {
    root.storageService = factory();
})(window, function () {

    const TOKEN_KEY = "token";

    return {

        saveToken(token) {
            console.log("Saving token:", token);
            localStorage.setItem(TOKEN_KEY, token);
        },

        getToken() {
            const token = localStorage.getItem(TOKEN_KEY);
            console.log("Reading token:", token);
            return token;
        },

        clearToken() {
            localStorage.removeItem(TOKEN_KEY);
        }
    };
});