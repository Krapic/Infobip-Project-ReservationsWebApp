import React from "react";
import DataCard from '../../scenes/adminWorkplace/DataCard';
import './BusinessData.css';
const BusinessData = ({ data }) =>{
    return(
        <div style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
            <h2 className="text" style={{ color: '#70D8BD', textAlign: 'center', fontSize: '35px' }} >Informacije o obrtu</h2>
            {data && data.Address && data.BusinessActivities && data.BusinessActivities.$values && <div style={{ marginTop: '5%', fontSize: '10px', font: 'Arial', fontWeight: '600' }}>
                <DataCard header='Ime obrta:' data={data.BusinessName} />
                <div style={{ marginTop: '2%' }}>
                    <DataCard header='Identifikacijski broj obrta:' data={data.BusinessIdentificationNumber} />
                </div>

                <div style={{ marginTop: '2%' }}>
                    <DataCard header='Usluge:' 
                        data={data.BusinessActivities.$values.map(activity => 
                        `${activity.NameOfActivity} - 
                        ${activity.DescriptionOfActivity} - 
                        ${activity.Price}€`)}
                    />
                </div>
            </div>}
        </div>
    )
}

export default BusinessData;