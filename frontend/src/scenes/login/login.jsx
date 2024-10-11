import React, { useState } from 'react'
import './login.css'
import { useTheme } from "@mui/material";
import { tokens } from '../../theme';
import emailImg from './assets/email.png'
import passwordImg from './assets/password.png'
import person from './assets/person.png'
import telephone from './assets/telephone.png'
import city from './assets/city.png'
import countryImg from './assets/country.png'
import postalcodeImg from './assets/postalcode.png'
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router-dom';
import { UserContext } from './UserContext';

const Login = ({ setEmailGlobal }) => {
    const navigate = useNavigate();
    const [action, setAction] = useState("Registriraj se");
    const theme = useTheme(); // Ovo služi za pristupanje temi koja je definirana u App.js
    const colors = tokens(theme.palette.mode); // Ovo služi za pristupanje bojama koje su definirane u App.js
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [town, setTown] = useState("");
    const [country, setCountry] = useState("");
    const [postalCode, setPostalCode] = useState("");
    const [dialingCode, setDialingCode] = useState("");
    const [houseNumber, setHouseNumber] = useState("");
    const [street, setStreet] = useState("");

    const handleLogin = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/auth/login/request`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) {
                // Handle failed login attempt
                console.error('Login failed:', response.statusText);
                alert('Neuspješna prijava: Krivi email ili lozinka');
                return;
            }
    
            const data = await response.json();
            console.log('Response data:', data);

            let jwt;
            if (email === 'test@gmail.com') {
            // If user is an admin, get JWT from the first API response
                jwt = data.jwt;
                } else {
                const sixDigitCode = prompt('Enter your 6-digit code');
                // Send the code to the server for verification
                const verifyResponse = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/auth/login/auth`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ email, sixDigitCode })
                });

        if (!verifyResponse.ok) {
            alert('Incorrect code');
            return;
        }
        // Get JWT from the second API response
        const verifyData = await verifyResponse.json();
        jwt = verifyData.jwt;
        }
            console.log('JWT:', data.jwt);
            // Save JWT to cookie
            Cookies.set('jwt', jwt, { sameSite: 'Lax', secure: false });
            // Save email to cookie
            Cookies.set('email', email, { sameSite: 'Lax', secure: false });
            
            navigate('/');
            window.location.reload();
 
        } catch (error) {
            console.error('Fetch error:', error);
        }
        setEmailGlobal(email);
    }
 
    const handleRegister = async () => {
        try {
            const userData = {
                name: name,
                lastName: lastName,
                email: email,
                password: password,
                phoneNumber: phoneNumber,
                town: town,
                country: country,
                postalCode: postalCode,
                dialingCode: dialingCode,
                houseNumber: houseNumber,
                street: street,
            };
    
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/auth/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            });
    
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
    
            const data = await response.json();
            console.log('Registration response:', data);
    
            // Handle successful registration (e.g., navigate to login page)
            setAction("Prijavi se");
    
        } catch (error) {
            console.error('Registration error:', error);
        }
    }

    const handleForgotPassword = async () => {
        const email = prompt("Unesite svoju email adresu:");
        if (email) {
            try {
                const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/auth/change-password/request`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ email })
                });
                const data = await response.json();
    
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}, message: ${data.response}`);
                }
                alert(`Link za promjenu lozinke poslan je na ${email}. Molimo provjerite svoj inbox.`); // Replace 'message' with the actual key in the response body
                console.log(data.response);
            } catch (error) {
                console.error('Forgot password error:', error);
                const errorMessage = error.message.split(',').pop().split(':').pop().trim(); // Get the last part of the error message
                alert(`Došlo je do pogreške: ${errorMessage}`); // Display the error message to the user
            }
        }
    }
 
  return (
    <UserContext.Provider value={email}>
    <div className ="container" backgroundcolor={colors.primary[400]} borderradius='10px'> {/* Ovo je primjer kako se pristupa bojama koje su definirane u App.js */}
        <div className="header">
            <div className="text" style={{ color: '#70D8BD' }}>{ action }</div>
            <div className="underline" style={{ backgroundColor: '#70D8BD' }}></div>
        </div>
        <div className="inputs">
            { action === "Prijavi se" ? <div></div> :<> <div className="input">
            <img src={person} alt="" />
            <input type="text" placeholder="Ime" value={name} onChange={e => setName(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={person} alt="" />
            <input type="text" placeholder="Prezime" value={lastName} onChange={e => setLastName(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={telephone} alt="" />
            <input type="phone-number" placeholder="Broj mobitela" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={telephone} alt="" />
            <input type="phone-number" placeholder="Pozivateljski broj (+385)" value={dialingCode} onChange={e => setDialingCode(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={city} alt="" />
            <input type="text" placeholder="Grad" value={town} onChange={e => setTown(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={countryImg} alt="" />
            <input type="text" placeholder="Država" value={country} onChange={e => setCountry(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={postalcodeImg} alt="" />
            <input type="text" placeholder="Poštanski broj" value={postalCode} onChange={e => setPostalCode(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={postalcodeImg} alt="" />
            <input type="text" placeholder="Ulica" value={street} onChange={e => setStreet(e.target.value)} required/>
        </div>
        <div className="input">
            <img src={postalcodeImg} alt="" />
            <input type="number" placeholder="Kućni broj" value={houseNumber} onChange={e => setHouseNumber(e.target.value)} required/>
        </div>
        </>
            }
            <div className="input">
                <img src={emailImg} alt="" />
                <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required/>
            </div>
            <div className="input">
                <img src={passwordImg} alt="" />
                <input type="password" placeholder="Lozinka" value={password} onChange={e => setPassword(e.target.value)} required/>
            </div>
        </div>
        {action === "Registriraj se" ? <div></div> : <div className="forgot-password" >Zaboravili ste lozinku? <span onClick={handleForgotPassword}>Klikni ovdje!</span></div>}
        <div className="submit-container">
            <div className={action=== "Prijavi se" ? "gray" : "submit"} onClick={action !== "Prijavi se" ? handleRegister : () => setAction("Registriraj se")}>Registriraj se</div>
            <div className={action ==="Registriraj se" ? "gray" : "submit"} onClick={action !== "Registriraj se" ? handleLogin : () => setAction("Prijavi se")}>Prijavi se</div>
        </div>
    </div>
    </UserContext.Provider>
  )
}
 
export default Login
 