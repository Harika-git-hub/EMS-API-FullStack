window.authService = (function () {

    const API_URL = "http://localhost:5000/api/auth";

    return {

        async login(username, password) {
            try {

                console.log("LOGIN CALLED");

                const res = await fetch(`${API_URL}/login`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ username, password })
                });

                const data = await res.json();

                console.log("LOGIN RESPONSE:", data);

                // 🔥 token extraction (safe)
                const token =
                    data.token ||
                    data.data?.token ||
                    data.jwt ||
                    data.accessToken;

                if (token) {

                    storageService.saveToken(token);
                    localStorage.setItem("username", username);

                    console.log("TOKEN SAVED:", storageService.getToken());

                    return { success: true };
                }

                return {
                    success: false,
                    message: "Token not received from backend"
                };

            } catch (err) {
                console.error("Login Error:", err);
                return {
                    success: false,
                    message: "Server error"
                };
            }
        },

        logout() {
            storageService.clearToken();
            localStorage.removeItem("username");
        },

        isAuthenticated() {
            return !!storageService.getToken();
        }
    };

})();