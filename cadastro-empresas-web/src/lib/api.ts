import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5114",
  withCredentials: true,
});

export default api;
