import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import registerApi from "./registerApi";
import {theme} from '../../theme';
// @ts-ignore
import basLogo from '../../images/logo.png';
import {FormHelperText} from "@mui/material";
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";


export default function Register() {

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");
    const [regSuccess, setRegSuccess] = React.useState(false);
    let navigate = useNavigate();



    useEffect(() => {
        if(regSuccess) {
            const timer = setTimeout(() => {
                navigate('/login')
            }, 3000);
            return () => clearTimeout(timer);
        }}, [navigate, regSuccess]);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const request = {
            firstName : data.get('firstName'),
            lastName : data.get('lastName'),
            password: data.get('password'),
            email: data.get('email'),
            phone: data.get('phone'),
            role: "CUSTOMER"
        }
        console.log(request);
        registerApi.register(request).then((response) => {
            if(response.status === 200) {
                setRegSuccess(true);
                setSuccessMessage("Registration successful. You will be redirected to login...");
                window.location.href = "/";
            }
        }).catch((error) => {
            setRegSuccess(false);
            setErrorMessage(error.response.data.message);
        });
    };

    // @ts-ignore
    return (
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs" style={{height: '100%'}}>
                <CssBaseline />
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Box
                        sx={{
                            mt: 10,
                            mb:2,
                            height: '4vh',
                            width: '100%',
                            backgroundImage: `url(${basLogo})`,
                            backgroundSize: 'cover',
                            backgroundPosition: 'center',
                            backgroundRepeat: 'no-repeat'
                        }}>
                    </Box>
                    <Typography component="h1" variant="h5">
                        Register
                    </Typography>
                    <Box component="form"  onSubmit={handleSubmit} sx={{ mt: 3}}>
                        <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    fullWidth
                                    id="firstName"
                                    label="First Name"
                                    name="firstName"
                                    autoComplete="given-name"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    fullWidth
                                    id="lastName"
                                    label="Last Name"
                                    name="lastName"
                                    autoComplete="family-name"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    fullWidth
                                    id="phone"
                                    label="Phone Number"
                                    name="phone"
                                    autoComplete="phone"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    fullWidth
                                    id="email"
                                    label="Email Address"
                                    name="email"
                                    autoComplete="email"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    color="success"
                                    required
                                    fullWidth
                                    name="password"
                                    label="Password"
                                    type="password"
                                    id="password"
                                    autoComplete="new-password"
                                />
                            </Grid>
                        </Grid>
                        <Box sx={{mt: 3, display: 'flex', justifyContent: 'center'}}>

                            <FormHelperText error>
                                {errorMessage}
                            </FormHelperText>

                        </Box>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2}}
                        >
                            Sign Up
                        </Button>
                        <Grid container justifyContent="flex-end">
                            <Grid item>
                                <Link href="/" variant="body2">
                                    Already have an account? Sign in
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}
