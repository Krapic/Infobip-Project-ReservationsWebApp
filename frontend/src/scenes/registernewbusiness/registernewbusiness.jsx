import React, { useState } from 'react';
import { useTheme } from "@mui/material";
import { tokens } from '../../theme';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Button from '@mui/material/Button';
import Cookies from 'js-cookie';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';

import './registernewbusiness.css';

const days = ['pon', 'uto', 'sri', 'cet', 'pet', 'sub', 'ned'];
const jwt = Cookies.get('jwt');
const RegisterBusiness = () => {
    const theme = useTheme();
    const colors = tokens(theme.palette.mode);
    const [action, ] = useState("Registriraj posao");
    const [open, setOpen] = useState(false);
    const [tempHours, setTempHours] = useState({ open: '0', close: '0' });
    const [workingHours, setWorkingHours] = useState({
        pon: { open: '0', close: '0' },
        uto: { open: '0', close: '0' },
        sri: { open: '0', close: '0' },
        cet: { open: '0', close: '0' },
        pet: { open: '0', close: '0' },
        sub: { open: '0', close: '0' },
        ned: { open: '0', close: '0' },
    });
    const [editingDay, setEditingDay] = useState(null);
    const [workingDays, setWorkingDays] = useState({
        pon: false,
        uto: false,
        sri: false,
        cet: false,
        pet: false,
        sub: false,
        ned: false
    });
    const [businessActivities, setBusinessActivities] = useState([
        {
            nameOfActivity: "", 
            descriptionOfActivity: "", 
            price: 0
        }
    ]);
    const [phone, setPhone] = useState("");
    const [city, setCity] = useState("");
    const [country, setCountry] = useState("");
    const [street, setStreet] = useState("");
    const [houseNumber, setHouseNumber] = useState("");
    const [postalCode, setPostalCode] = useState("");
    const [jobName, setJobName] = useState("");
    const [businessType, setBusinessType] = useState("");
    const handleStreetChange = (e) => setStreet(e.target.value);
    const handleHouseNumberChange = (e) => setHouseNumber(e.target.value);
    const handleJobNameChange = (e) => setJobName(e.target.value);
    const handlePhoneNumberChange = (e) => setPhone(e.target.value);
    const handleTownChange = (e) => setCity(e.target.value);
    const handleCountryChange = (e) => setCountry(e.target.value);
    const handlePostalCodeChange = (e) => setPostalCode(e.target.value);
    const [newDialogOpen, setNewDialogOpen] = useState(false);
    const [dodaneUsluge, setDodaneusluge] = useState(false);
    const [firstInput, setFirstInput] = useState("");
    const [secondInput, setSecondInput] = useState("");
    const [thirdInput, setThirdInput] = useState("");
    const handleFirstInputChange = (e) => setFirstInput(e.target.value);
    const handleSecondInputChange = (e) => setSecondInput(e.target.value);
    const handleThirdInputChange = (e) => setThirdInput(e.target.value);
    const handleNewDialogOpen = () => setNewDialogOpen(true);
    const handleNewDialogClose = () => setNewDialogOpen(false);
    const handleDodaneUslugeOpen = () => setDodaneusluge(true);
    const handleDodaneUslugeClose = () => setDodaneusluge(false);

    const handleWorkingDaysChange = (e) => {
        const { target } = e;
        const { name, checked } = target;
        if (checked) {
            setEditingDay(name);
            setOpen(true);
        } else {
            setWorkingDays(prevState => ({
                ...prevState,
                [name]: false
            }));
            setWorkingHours(prevState => {
                const newState = { ...prevState };
                delete newState[name];
                return newState;
            });
        }
    };
    const handleWorkingHoursChange = (e) => {
        const { target } = e;
        const { value, name } = target;
        const time = name.split('-')[1];
        setTempHours(prevState => ({
            ...prevState,
            [time]: value
        }));
    }
    const handleConfirm = () => {
        setWorkingDays(prevState => ({
            ...prevState,
            [editingDay]: true
        }));
        setWorkingHours(prevState => ({
            ...prevState,
            [editingDay]: tempHours
        }));
        setTempHours({ open: '', close: '' }); // Reset tempHours
        setEditingDay(null);
        setOpen(false);
    };
    const handleClose = () => {
        setEditingDay(null);
        setOpen(false);
    };
    const handleSubmit = () => {
        const data = {
            userEmail: Cookies.get('email'),
            userPhoneNumber: phone,
            town: city,
            country: country,
            street: street,
            houseNumber: houseNumber,
            postalCode: postalCode,
            businessName: jobName,
            businessIdentificationNumber: Math.floor(Math.random() * (1000000 - 1 + 1)) + 1, 
            businessType: businessType, 
            businessActivities: businessActivities,
            workingDays: Object.values(workingDays),
            startingHours: Object.values(workingHours).map(hours => parseInt(hours.open.split(':')[0])),
            endingHours: Object.values(workingHours).map(hours => parseInt(hours.close.split(':')[0]))
        };
        console.log(businessActivities);
        //const BACKEND_URL = process.env.BACKEND_URL;
        fetch(`${process.env.REACT_APP_BACKEND_URL}/api/business/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwt}`,
            },
            body: JSON.stringify(data),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            console.log(data);
            return response.json();
        })
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
            console.log(data);
        });
    };

    const handleNewDialogConfirm = () => {
        setBusinessActivities(prevActivities => [
            ...prevActivities,
            {
                nameOfActivity: firstInput, 
                descriptionOfActivity: secondInput, 
                price: thirdInput
            }
        ]);
        setFirstInput("");
        setSecondInput("");
        setThirdInput(0);
        setNewDialogOpen(false);
    };

    return (
        <div className="container" backgroundcolor={colors.primary[400]} borderradius='10px'>
            <div className="header">
                <div className="text"  style={{ color: '#70D8BD' }} >{ action }</div>
                <div className="underline"  style={{ backgroundColor: '#70D8BD' }} ></div>
            </div>
            <div className="inputs">
                <Select
                    value={businessType}
                    onChange={(event) => setBusinessType(event.target.value)}
                    displayEmpty
                    inputProps={{ 'aria-label': 'Without label' }}
                    >
                    <MenuItem value="">
                        <em>Vrsta posla</em>
                    </MenuItem>
                    <MenuItem value={1}>Šišanje</MenuItem>
                    <MenuItem value={2}>Masiranje</MenuItem>
                </Select>
                <Input type="text" placeholder="Ime posla" required value={jobName} onChange={handleJobNameChange} />   
                <Input type="number" placeholder="Broj mobitela" required value={phone} onChange={handlePhoneNumberChange} />
                <Input type="text" placeholder="Ulica" required value={street} onChange={handleStreetChange} />
                <Input type="number" placeholder="Kućni broj" required value={houseNumber} onChange={handleHouseNumberChange} />
                <Input type="text" placeholder="Grad" required value={city} onChange={handleTownChange} />
                <Input type="text" placeholder="Država" required value={country} onChange={handleCountryChange} />
                <Input type="number" placeholder="Poštanski broj" required value={postalCode} onChange={handlePostalCodeChange} />     
                <div className="input">
                {days.map(day => (
                    <React.Fragment key={day}>
                        <input 
                            type="checkbox" 
                            id={day} 
                            name={day} 
                            checked={workingDays[day]} 
                            onChange={handleWorkingDaysChange}
                        />
                        <label htmlFor={day}>{day.toUpperCase()}</label>
                    </React.Fragment>
                ))}
                </div>
                <button onClick={handleNewDialogOpen} className='dodajNovuUslugu'>Dodaj novu uslugu</button>
            </div>
            <Dialog open={open} onClose={handleClose} fullWidth={true} maxWidth="sm">
                <DialogTitle sx={{ fontSize: '20px' }} >{`Odredite radno vrijeme za ${editingDay ? editingDay.toUpperCase() : ''}`}</DialogTitle>
                    <DialogContent>
                        <DialogContentText sx={{ fontSize: '10px' }}>
                            Odaberite od kad do kad radite za {editingDay ? editingDay.toUpperCase() : ''}.
                        </DialogContentText>    
                        <DialogContentText style={{ display: 'flex', alignItems: 'center', fontSize: '20px' }} >
                            <input style={{ marginLeft: '3%' }} type="time" name={`${editingDay}-open`} placeholder="Otvoreno od" onChange={handleWorkingHoursChange}/>
                            <input style={{ marginLeft: '3%' }} type="time" name={`${editingDay}-close`} placeholder="Zatvoreno od" onChange={handleWorkingHoursChange}/>
                        </DialogContentText>
                    </DialogContent>
                <DialogActions>
                    <Button sx={{ fontSize: '12px', padding: '8px 16px' }} onClick={handleClose}>Close</Button>
                    <Button sx={{ fontSize: '12px', padding: '8px 16px' }} onClick={handleConfirm}>Confirm</Button>
                </DialogActions>
            </Dialog>
            <Dialog open={newDialogOpen} onClose={handleNewDialogClose} fullWidth={true} maxWidth="sm">
                <DialogTitle>Dodaj uslugu</DialogTitle>
                <DialogContent style={{ fontSize: '25px' }}>
                    <input type="text" value={firstInput} placeholder="Usluga" onChange={handleFirstInputChange} />
                    <input type="text" value={secondInput} placeholder="Opis usluge" onChange={handleSecondInputChange} />
                    <input type="number" value={thirdInput} placeholder="Cijena usluge u €" onChange={handleThirdInputChange} />
                </DialogContent>
                <DialogActions>
                    <Button sx={{ fontSize: '12px', padding: '8px 16px' }} onClick={handleNewDialogClose}>Close</Button>
                    <Button sx={{ fontSize: '12px', padding: '8px 16px' }} onClick={handleNewDialogConfirm}>Confirm</Button>
                    <Button sx={{ fontSize: '12px', padding: '8px 16px' }} onClick={handleDodaneUslugeOpen}>Dodane usluge</Button>
                    <Dialog open={dodaneUsluge} onClose={handleDodaneUslugeClose} fullWidth={true} maxWidth="sm">
                        <DialogTitle>Dodane usluge</DialogTitle>
                        <DialogContent style={{ fontSize: '55px' }}>
                            <DialogContentText>
                                {businessActivities.map((activity, index) => (
                                    <div key={index}>
                                        <h2>{activity.nameOfActivity}</h2>
                                        <p>{activity.descriptionOfActivity}</p>
                                        <p>{activity.price}€</p>
                                    </div>
                                ))}
                            </DialogContentText>
                        </DialogContent>
                    </Dialog>
                </DialogActions>
            </Dialog>
            <div className="submit-container">
                <div className={action=== "Prijavi posao" ? "submit gray" : "submit"} onClick={handleSubmit}>Registriraj posao</div>
            </div>
        </div>
    )
}

const Input = ({ type, placeholder, ...props }) => (
    <div className="input">
        <img src="" alt="" />
        <input type={type} placeholder={placeholder} {...props} />
    </div>
);

export default RegisterBusiness;