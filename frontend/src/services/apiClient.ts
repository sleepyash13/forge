import axios, { AxiosError } from "axios";
import { getCookie } from "../utils/cookie";

const API_BASE_URL = `${import.meta.env.VITE_API_URL}/api/v1`;

export const apiClient = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
    },
});

apiClient.interceptors.request.use((config) => {
    const method = config.method?.toLowerCase();

    const needsCsrf =
        method === "post" ||
        method === "put" ||
        method === "patch" ||
        method === "delete";

    if (needsCsrf) {
        const csrfToken = getCookie("XSRF-TOKEN");

        if (csrfToken) {
            config.headers["X-CSRF-TOKEN"] = csrfToken;
        }
    }

    return config;
});

apiClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError<{ message?: string }>) => {
        const message =
            error.response?.data?.message ??
            "An unexpected error occurred.";

        console.error(message);

        return Promise.reject(error);
    }
);