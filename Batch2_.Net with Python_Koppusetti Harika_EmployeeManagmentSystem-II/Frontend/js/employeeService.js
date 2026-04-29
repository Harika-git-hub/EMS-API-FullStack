window.employeeService = (function () {

    const API_URL = "http://localhost:5000/api/employees"; // CHANGE PORT TO MATCH SWAGGER

    async function getAll() {
        try {
            const token = storageService.getToken();
            const res = await fetch(`${API_URL}?pageSize=1000`, {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + token
                }
            });
            if (!res.ok) return [];
            const data = await res.json();
            return Array.isArray(data?.data) ? data.data : [];
        } catch (err) {
            console.error("getAll Error:", err);
            return [];
        }
    }

    async function getDashboardSummary() {
        try {
            const token = storageService.getToken();
            const res = await fetch(`${API_URL}/dashboard-summary`, {
                headers: { "Authorization": "Bearer " + token }
            });
            if (!res.ok) return null;
            return await res.json();
        } catch (err) {
            console.error("Summary Error:", err);
            return null;
        }
    }

    async function addEmployee(employeeData) {
        const token = storageService.getToken();
        const res = await fetch(`${API_URL}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify(employeeData)
        });
        if (!res.ok) {
            const errorText = await res.text();
            throw new Error(errorText || 'Failed to add employee');
        }
        return await res.json();
    }

    return {
        getAll,
        getDashboardSummary,
        addEmployee  // THIS LINE MAKES IT WORK
    };

})();