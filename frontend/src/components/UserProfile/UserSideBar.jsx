import React from "react";
import { Link } from 'react-router-dom';
import './UserSideBar.css';
import PersonOutlineIcon from '@mui/icons-material/PersonOutline';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BusinessIcon from '@mui/icons-material/Business';
import LockIcon from '@mui/icons-material/Lock';
import { useTheme } from "@mui/material";
import useMediaQuery from '@mui/material/useMediaQuery';

const UserSideBar = ({activepage}) =>{
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down(590));
    return(
        <div className="usersidebar" >
            {
                activepage === 'accountsettings' ?
                    <div className="s2"
                        style={{ backgroundColor: theme.palette.mode === 'dark' ? '#1C1C1C' : 'rgb(246, 246, 246)' }}
                    >
                        <PersonOutlineIcon className="responsive-icons" />
                        <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Informacije o korisniku</span>
                    </div>
                    :
                    <Link 
                        to='/user/accountsettings'
                        className="stylenone"
                        style={{ textDecoration: 'none' }}
                    >
                        <div className="s1">
                        <PersonOutlineIcon className="responsive-icons" />
                            <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Informacije o korisniku</span>
                        </div>
                    </Link>
            }

            {
                activepage === 'yourorders' ?
                <div className="s2"
                        style={{ backgroundColor: theme.palette.mode === 'dark' ? '#1C1C1C' : 'rgb(246, 246, 246)' }}
                >
                    <AssignmentIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Moje rezervacije</span>
                </div>
                :
                <Link 
                    to='/user/yourorders'
                    className="stylenone"
                    style={{ textDecoration: 'none' }}
                >
                <div className="s1">
                    <AssignmentIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Moje rezervacije</span>
                </div>
              </Link>
            }

            {
                activepage === 'changepassword' ?
                <div className="s2"
                        style={{ backgroundColor: theme.palette.mode === 'dark' ? '#1C1C1C' : 'rgb(246, 246, 246)' }}
                >
                    <LockIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Promijeni lozinku</span>
                </div>
                :
                <Link 
                    to='/user/changepassword'
                    className="stylenone"
                    style={{ textDecoration: 'none' }}
                >
                <div className="s1">
                <LockIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Promijeni lozinku</span>
              </div>
              </Link>
            }

            {
                activepage === 'business' ?
                <div className="s2"
                        style={{ backgroundColor: theme.palette.mode === 'dark' ? '#1C1C1C' : 'rgb(246, 246, 246)' }}
                >
                    <BusinessIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Moj obrt</span>
                </div>
                :
                <Link 
                    to='/user/business'
                    className="stylenone"
                    style={{ textDecoration: 'none' }}
                >
                <div className="s1">
                <BusinessIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Moj obrt</span>
              </div>
              </Link>
            }

            {
                activepage === 'businessorders' ?
                <div className="s2"
                        style={{ backgroundColor: theme.palette.mode === 'dark' ? '#1C1C1C' : 'rgb(246, 246, 246)' }}
                >
                    <AssignmentIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Rezervacije za moj obrt</span>
                </div>
                :
                <Link 
                    to='/user/businessorders'
                    className="stylenone"
                    style={{ textDecoration: 'none' }}
                >
                <div className="s1">
                    <AssignmentIcon className="responsive-icons" />
                    <span style={{ fontSize: isMobile ? '10px' : '15px' }} >Rezervacije za moj obrt</span>
                </div>
              </Link>
            }
        </div>
    )
}

export default UserSideBar;