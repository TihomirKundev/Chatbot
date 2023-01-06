import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Paper from '@mui/material/Paper';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
// @ts-ignore
import bas from '../../images/BAS-World-HQ-0.jpg';
// @ts-ignore
import basLogo from '../../images/logo.png';
import { ThemeProvider } from '@mui/material/styles';
import {theme} from '../../theme';
import userApi from './userApi';
import {FormHelperText} from "@mui/material";


export default function LoginPage() {

    const [errorMessage, setErrorMessage] = React.useState("");

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const request = {
            email: data.get('email'),
            password: data.get('password'),
        }

        userApi.login(request).then(() => {
            const user = userApi.getCurrentUser();
            if (user.role === "ADMIN") {
                window.location.href = "/admin";
            } else if (user.role === "CUSTOMER") {
                window.location.href = "/customer";
            }
        }).catch((error) => {
            setErrorMessage(error.response.data.message);
        });
    };

    return (
        <ThemeProvider  theme={theme}>
            <Grid container component="main" style={{height: '100%'}}>
                <CssBaseline />
                <Grid
                    item
                    xs={false}
                    sm={4}
                    md={7}
                    sx={{
                        backgroundImage: `url(${bas})`,
                        backgroundRepeat: 'no-repeat',
                        backgroundSize: 'cover',
                        backgroundPosition: 'center',
                    }}
                />
                <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square >
                    <Box
                        sx={{
                            my: 8,
                            mx: 4,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                        }}
                    >
                        <Box
                            sx={{
                                height: '3vh',
                                width: '40%',
                                backgroundImage: `url(${basLogo})`,
                                backgroundSize: 'cover',
                                backgroundPosition: 'center',
                                backgroundRepeat: 'no-repeat'
                            }}>
                        </Box>
                        <Typography component="h1" variant="h5">
                            Sign in
                        </Typography>
                        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1}}>
                            <TextField
                                margin="normal"
                                required
                                fullWidth
                                id="email"
                                label="Email Address"
                                name="email"
                                autoComplete="email"
                                autoFocus
                            />
                            <TextField
                                margin="normal"
                                required
                                fullWidth
                                name="password"
                                label="Password"
                                type="password"
                                id="password"
                                autoComplete="current-password"
                            />
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
                                Sign In
                            </Button>
                            <Grid container>
                                <Grid item>
                                    <Link href="/register" variant="body2">
                                        {"Don't have an account? Sign Up"}
                                    </Link>
                                </Grid>
                            </Grid>
                        </Box>
                    </Box>
                </Grid>
            </Grid>
        </ThemeProvider>
    );
}
