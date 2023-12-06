const getResponse = async (apiPromise: Promise<Response>) => {
	const response = await apiPromise;
	const contentType = response.headers.get("Content-Type") || "";
	if (response.ok === true) {
		if (response.status === 204) return null;
		if (contentType.includes("application/json")) return response.json();
		return response.text();
	}
	let message = "";
	if (contentType.includes("application/json")) {
		const json = await response.json();
		message = json.message || JSON.stringify(json);
	} else if (contentType.includes("text/plain")) {
		message = await response.text();
	}
	if (!message) message = response.statusText;
	throw new Error(`${response.status}${message ? ": " + message : ""}`);
};

export const get = <T>(url: string): Promise<T> => {
	return getResponse(
		fetch(url, {
			credentials: "same-origin",
			headers: {
				Accept: "application/json",
				Authorization: `Bearer ${localStorage.getItem("jwt")}`,
			},
		}),
	);
};

export const post = <T>(url: string, body: unknown): Promise<T> => {
	return getResponse(
		fetch(url, {
			body: JSON.stringify(body),
			credentials: "same-origin",
			headers: {
				Accept: "application/json",
				"Content-Type": "application/json",
				Authorization: `Bearer ${localStorage.getItem("jwt")}`,
			},
			method: "POST",
		}),
	);
};

export const GATEWAY_URI =
	import.meta.env.MODE === "development" ? "https://localhost:7777" : "https://wingridgateway.azurewebsites.net";

export const AUTH_URI =
	import.meta.env.MODE === "development" ? "https://localhost:7001" : "https://wingridauthapi.azurewebsites.net";
