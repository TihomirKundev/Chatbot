import axios from "axios";
import {BaseUrl, LoginApi} from "../../app.properties";

const login = async (request) => {
    return await axios.post(BaseUrl + LoginApi, request).then((response) => {
        if (response.data) {
            localStorage.setItem('user', JSON.stringify(response.data));
        }
        return response.data;
    })
};

const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem('user')) || null;
};

export default {
    login,
    getCurrentUser
}
