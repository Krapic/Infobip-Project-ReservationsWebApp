import React from "react";
import DataCard from '../../scenes/adminWorkplace/DataCard';
import './AccountSettings.css';

const AccountSettings = ({ data }) =>{
    return(
        <div style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
            <h2 className="text" style={{ color: '#70D8BD', textAlign: 'center', fontSize: '35px' }} >Informacije o korisniku</h2>
            {data && data.Address && <div style={{ marginTop: '5%', fontSize: '10px', font: 'Arial', fontWeight: '600' }}>
                <DataCard header='Ime i prezime:' data={data.Name} />

                <div style={{ marginTop: '2%' }}>
                    <DataCard header='Adresa:' data={data.Address.Country + 
                        ", " + data.Address.PostalCode + 
                        ", " + data.Address.Town +
                        ", " + data.Address.Street + " " + data.Address.HouseNumber} />
                </div>

                <div style={{ marginTop: '2%' }}>
                    <DataCard header='Broj telefona:' data={data.DialingCode + 
                        " " + data.PhoneNumber} />
                </div>

                <div style={{ marginTop: '2%' }}>
                    <DataCard header='Email:' data={data.Email} />
                </div>
            </div>}
        </div>
    )
}

export default AccountSettings;