import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import AccountSettings from "../../components/UserProfile/AccountSettings";
import UserSideBar from "../../components/UserProfile/UserSideBar";
import './UserProfile.css';
import ChangePassword from "../../components/UserProfile/ChangePassword";
import MyOrders from "../../components/UserProfile/MyOrders";
import BusinessData from "../../components/UserProfile/BusinessData";
import BusinessOrders from "../../components/UserProfile/BusinessOrders";
import Cookies from "js-cookie";
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from "@mui/material";


const UserProfile = () =>{
    const {activepage} = useParams();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down(590));
    const userEmail = Cookies.get('email');
    const jwt = Cookies.get('jwt');
    const [profileData, setProfileData] = useState({});

    const fetchData = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/profile?Email=${userEmail}`, {
                method: 'GET',
                headers: {
                    'Authorization' : `Bearer ${jwt}`,
                    'Content-Type': 'application/json'
                }
            });
    
            const data = await response.json();
    
            setProfileData(data.data);
            
        } catch (error) {
            console.log(error);
        }
      };
    
      useEffect(() => {
        fetchData();
      }, []);

    return(
        <div className="userprofile">
            <div className="userprofilein">
                <div className="left" style={{ maxHeight: isMobile ? '85vh' : '50vh' }} >
                    <UserSideBar activepage={activepage}/>
                    
                </div>
                <div className="right" style={{ maxHeight: isMobile ? '85vh' : '50vh' }} >
                    {activepage === 'accountsettings' && <AccountSettings data={profileData} />}
                    {activepage === 'changepassword' && <ChangePassword />}
                    {activepage === 'yourorders' && <MyOrders />}
                    {activepage === 'business' && <BusinessData data={profileData} />}
                    {activepage === 'businessorders' && <BusinessOrders />}
                </div>
            </div>
        </div>
    )
}

export default UserProfile;