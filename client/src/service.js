import axios from 'axios';

// יצירת מופע מותאם אישית של Axios עם כתובת בסיס (Default Config)
const apiClient = axios.create({
  baseURL: "http://localhost:5269", // כתובת הבסיס
});

// הוספת Interceptor לתפיסת שגיאות בתגובה
apiClient.interceptors.response.use(
  (response) => response, // החזרת התגובה אם אין שגיאות
  (error) => {
    console.error("API Error:", error.response?.data || error.message); // רישום שגיאה ללוג
    return Promise.reject(error); // המשך זריקת השגיאה לשימוש ב-try/catch
  }
);

export default {
  getTasks: async () => {
    const result = await apiClient.get("/items"); // שימוש בכתובת הבסיס
    return result.data;
  },

  addTask: async (name) => {
    const body = { Name: name, IsComplete: false, Id: 0 };
    const result = await apiClient.post("/items", body); // שימוש בכתובת הבסיס
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    const body = { IsComplete: isComplete, Id: id };
    const result = await apiClient.put(`/items/${id}`, body); // שימוש בכתובת הבסיס
    return result.data;
  },

  deleteTask: async (id) => {
    const result = await apiClient.delete(`/items/${id}`); // שימוש בכתובת הבסיס
    return result.data;
  },
};
