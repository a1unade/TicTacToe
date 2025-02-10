/* istanbul ignore file */

import axios from 'axios';

// Axios-клиент для запросов к API
const apiClient = axios.create({
  baseURL: 'http://localhost:8080/',
});

export default apiClient;
