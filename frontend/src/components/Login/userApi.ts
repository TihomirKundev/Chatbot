import axios from "axios";
import {BaseUrl, LoginApi} from "../../app.properties";

interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
    token: string;
};

const login = async (request) : Promise<User> => {
    return await axios.post(BaseUrl + LoginApi, request).then((response) => {
        if (response.data) {
            localStorage.setItem('user', JSON.stringify(response.data));
        }
        return response.data;
    })
};

const getCurrentUser = () : User => {
    return JSON.parse(localStorage.getItem('user')) || null;
};

export default {
    login,
    getCurrentUser
}
