import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { useState } from 'react';
import './Filter.css';

const Filter = (props) => {
    const [selectedOption, setSelectedOption] = useState('');

    const handleFilterChange = (event) => {
        const selectedValue = event.target.value;
        setSelectedOption(selectedValue);

        const countryRegex = /&Country=[^&]*/;
        const townRegex = /&Town=[^&]*/;
        const businessTypeRegex = /&BusinessType=[^&]*/;
        let updatedQueryString = '';
        
        if (props.logicParameter === 1) {
            if(selectedValue === 'none'){
                updatedQueryString = props.queryString.replace(countryRegex, '');
            }else{
                if (props.queryString.includes("&Country=")) {
                    updatedQueryString = props.queryString.replace(countryRegex, `&Country=${selectedValue}`);
                } else {
                    updatedQueryString = `${props.queryString}&Country=${selectedValue}`;
                }
            }
        } else if (props.logicParameter === 2) {
            if(selectedValue === 'none'){
                updatedQueryString = props.queryString.replace(townRegex, '');
            }else{
                if (props.queryString.includes("&Town=")) {
                    updatedQueryString = props.queryString.replace(townRegex, `&Town=${selectedValue}`);
                } else {
                    updatedQueryString = `${props.queryString}&Town=${selectedValue}`;
                }
            }
            
        } else if (props.logicParameter === 3) {
            if(selectedValue === 'none'){
                updatedQueryString = props.queryString.replace(businessTypeRegex, '');
            }else{
                if (props.queryString.includes("&BusinessType=")) {
                    updatedQueryString = props.queryString.replace(businessTypeRegex, `&BusinessType=${selectedValue}`);
                } else {
                    updatedQueryString = `${props.queryString}&BusinessType=${selectedValue}`;
                }
            }
            
        }
        props.setQueryString(updatedQueryString);
        props.fetchData(updatedQueryString);
    };

    return (
        <FormControl variant="standard" sx={{ width: props.width, marginRight: '2%' }} >
            <InputLabel
                sx={{ fontSize: '13px' }}
            >{props.label}</InputLabel>
            <Select
                value={selectedOption}
                onChange={handleFilterChange}
                disabled={props.disabled}
                disableUnderline
            >
                <MenuItem sx={{ fontSize: '12px' }} value='none'>Prazno</MenuItem>
                {props.data && props.data.$values.map((x, index) => 
                    <MenuItem key={index} value={x} sx={{ fontSize: '12px' }} >{
                        x === 1 ? "Frizer" : (x === 2 ? "Masažer" : x)
                    }</MenuItem>
                )}
            </Select>
        </FormControl>
    );
}

export default Filter;