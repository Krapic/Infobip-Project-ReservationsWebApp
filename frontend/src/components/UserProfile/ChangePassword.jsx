import React from "react";
import { useTheme } from "@mui/material";
import './ChangePassword.css';

const ChangePassword = () =>{
    const theme = useTheme();
    return(
        <div style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
            <h2 className="text" style={{ color: '#70D8BD', textAlign: 'center', fontSize: '35px' }} >Promjeni lozinku</h2>

            <div style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', marginTop: '5%' }}>
                <div style={{ display: 'flex', flexDirection: 'column', textAlign: 'left', width: '50%', marginBottom: '5%' }}>
                    <label htmlFor="oldpass" style={{ fontSize: '15px' }} >Stara lozinka <span>*</span></label>
                    <input 
                        style={{
                            backgroundColor: theme.palette.mode === 'dark' ? '#333333' : '',
                            borderColor: theme.palette.mode === 'dark' ? '#333333' : 'white',
                            fontSize: '15px',
                            boxShadow: '0 6px 15px rgba(0, 0, 0, 0.3)',
                            border: 'none',
                            borderRadius: '10px',
                            padding: '5px'
                        }} 
                        type="password" />
                </div>

                <div style={{ display: 'flex', flexDirection: 'column', textAlign: 'left', width: '50%', marginBottom: '5%' }}>
                    <label htmlFor="newpass" style={{ fontSize: '15px' }}>Nova lozinka <span>*</span></label>
                    <input 
                        style={{
                            backgroundColor: theme.palette.mode === 'dark' ? '#333333' : '',
                            borderColor: theme.palette.mode === 'dark' ? '#333333' : 'white',
                            fontSize: '15px',
                            boxShadow: '0 6px 15px rgba(0, 0, 0, 0.3)',
                            border: 'none',
                            borderRadius: '10px',
                            padding: '5px'
                        }} 
                        type="password" />
                </div>

                <button className="submit-save-changes" style={{ backgroundColor: '#70D8BD' }} >Spremi promjene</button>
            </div>
            
        </div>
    )
}

export default ChangePassword;