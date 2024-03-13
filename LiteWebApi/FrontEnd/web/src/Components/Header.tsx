import {ErrorMessage, Field, Form, Formik} from "formik";
import {useRef, useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import {imagesDir, loginAsync, registrationAsync} from "../Shared/api.ts";
import {Box, Modal} from "@mui/material";
import styles from '../styles.module.css';
import ILoginData from "../Interfaces/Formik/ILoginData.tsx";
import IRegistrationData from "../Interfaces/Formik/IRegistrationData.tsx";
import style from '../modalStyles.ts';
import {useDispatch} from "react-redux";
import {AuthReducerActionType} from "../Redux/Reducer/AuthReducer.ts";
import IUserData from "../Interfaces/Token/IUserData.tsx";
import {jwtDecode} from "jwt-decode";
import {useAppSelector} from "../Redux/Hooks/hooks.ts";
import {Spin} from 'antd';
import axios from "axios";
import {ShoppingCartOutlined, ShoppingOutlined} from "@ant-design/icons";

function Header() {
    const dispatch = useDispatch();
    const auth = useAppSelector(store => store.auth);
    const navigate = useNavigate();

    const [errorMessage, setErrorMessage] = useState('');

    const image = useRef<File | null>(null);

    const [openLoginModal, setOpenLoginModal] = useState(false);
    const [openRegistrationModal, setOpenRegistrationModal] = useState(false);

    const loginForm = (
        <Formik<ILoginData>
            initialValues={{
                email: "",
                password: ""
            }}

            validate={(values: ILoginData) => {
                const errors = {} as any;
                const {email, password} = values;

                if (!email)
                    errors.email = 'Required';

                if (password.length < 5)
                    errors.password = 'The minimum password length is 5 characters';

                return errors;
            }}

            onSubmit={async (values, {setSubmitting}) => {
                const {email, password} = values;

                const formData = new FormData();
                formData.append('email', email);
                formData.append('password', password);

                try {
                    const result = await loginAsync(formData);
                    const {token} = result;

                    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
                    localStorage.token = token;

                    dispatch({
                        type: AuthReducerActionType.LOGIN_USER,
                        payload: jwtDecode<IUserData>(token)
                    });

                    setErrorMessage('');
                    setOpenLoginModal(false);
                } catch (error: any) {
                    setErrorMessage(`${error.response.data.title || error.response.data}`);
                }

                setSubmitting(false);
            }}
        >
            {({handleSubmit, isSubmitting}) => (
                <Spin spinning={isSubmitting}>
                    <Form onSubmit={handleSubmit} className='container-fluid'>
                        <div className="form-group">
                            <label htmlFor="emailInput">Email</label>
                            <Field name='email' type='text' className="form-control" id="emailInput"/>
                            <ErrorMessage name="email" component="div" className='text-danger'/>

                            <label htmlFor="passwordInput">Password</label>
                            <Field name='password' type="password" className="form-control" id="passwordInput"/>
                            <ErrorMessage name="password" component="div" className='text-danger'/>
                        </div>

                        <div>
                            <button type="submit" disabled={isSubmitting} className="btn btn-primary mt-4">Login
                            </button>
                            <button className="btn btn-primary mt-4" style={{marginLeft: 10}}
                                    onClick={() => setOpenLoginModal(false)}>Cancel
                            </button>
                        </div>

                        {errorMessage && <span className='text-danger'>{errorMessage}</span>}
                    </Form>
                </Spin>
            )}
        </Formik>
    );

    const loginModal = <Modal
        open={openLoginModal}
        onClose={() => {
            setOpenLoginModal(false);
            setErrorMessage('');
        }}
    >
        <Box sx={{...style}}>
            <h2>Login</h2>

            {loginForm}
        </Box>
    </Modal>

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const fileInput = event.target;
        if (fileInput.files && fileInput.files.length > 0) {
            image.current = fileInput.files[0];
        } else {
            image.current = null;
        }
    };

    const registrationForm = (
        <Formik<IRegistrationData>
            initialValues={{
                firstName: "",
                lastName: "",
                email: "",
                userName: "",
                password: ""
            }}

            validate={(values: IRegistrationData) => {
                const errors = {} as any;
                const {firstName, lastName, email, userName, password} = values;

                if (!firstName)
                    errors.firstName = 'Required';
                if (firstName.startsWith(' '))
                    errors.firstName = 'Starts with space symbol';
                if (firstName.endsWith(' '))
                    errors.firstName = 'Ends with space symbol';
                if (firstName.length > 100)
                    errors.firstName = 'Too long first name';

                if (!lastName)
                    errors.lastName = 'Required';
                if (lastName.startsWith(' '))
                    errors.lastName = 'Starts with space symbol';
                if (lastName.endsWith(' '))
                    errors.lastName = 'Ends with space symbol';
                if (lastName.length > 100)
                    errors.lastName = 'Too long last name';

                if (!email)
                    errors.email = 'Required';

                if (!userName)
                    errors.userName = 'Required';
                if (userName.length > 30)
                    errors.userName = 'Too long user name';

                if (!image.current)
                    errors.image = 'Required';

                if (!firstName)
                    errors.firstName = 'Required';
                if (password.length < 5)
                    errors.password = 'The minimum password length is 5 characters';

                return errors;
            }}

            onSubmit={async (values, {setSubmitting}) => {
                const {firstName, lastName, email, userName, password} = values;

                const formData = new FormData();
                formData.append('firstName', firstName);
                formData.append('lastName', lastName);
                formData.append('email', email);
                formData.append('userName', userName);
                if (image.current) formData.append('image', image.current);
                formData.append('password', password);

                try {
                    const result = await registrationAsync(formData);
                    const {token} = result;

                    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
                    localStorage.token = token;

                    dispatch({
                        type: AuthReducerActionType.LOGIN_USER,
                        payload: jwtDecode<IUserData>(token)
                    });

                    setErrorMessage('');
                    setOpenRegistrationModal(false);
                } catch (error: any) {
                    setErrorMessage(`${error.response.data.title || error.response.data}`);
                }

                setSubmitting(false);
            }}
        >
            {({handleSubmit, isSubmitting}) => (
                <Spin spinning={isSubmitting}>
                    <Form onSubmit={handleSubmit} className='container-fluid'>
                        <div className="form-group">
                            <label htmlFor="firstNameInput">First name</label>
                            <Field name='firstName' type='text' className="form-control" id="firstNameInput"/>
                            <ErrorMessage name="firstName" component="div" className='text-danger'/>

                            <label htmlFor="lastNameInput">Last name</label>
                            <Field name='lastName' type='text' className="form-control" id="lastNameInput"/>
                            <ErrorMessage name="lastName" component="div" className='text-danger'/>

                            <label htmlFor="emailInput">Email</label>
                            <Field name='email' type='text' className="form-control" id="emailInput"/>
                            <ErrorMessage name="email" component="div" className='text-danger'/>

                            <label htmlFor="userNameInput">User name</label>
                            <Field name='userName' type='text' className="form-control" id="userNameInput"/>
                            <ErrorMessage name="userName" component="div" className='text-danger'/>

                            <label htmlFor="imageInput">Image</label>
                            <Field name='image' type="file" className="form-control" id="imageInput"
                                   onChange={handleFileChange}/>
                            <ErrorMessage name="image" component="div" className='text-danger'/>

                            <label htmlFor="passwordInput">Password</label>
                            <Field name='password' type="password" className="form-control" id="passwordInput"/>
                            <ErrorMessage name="password" component="div" className='text-danger'/>
                        </div>

                        <div>
                            <button type="submit" disabled={isSubmitting} className="btn btn-primary mt-4">Registration
                            </button>
                            <button className="btn btn-primary mt-4" style={{marginLeft: 10}}
                                    onClick={() => setOpenRegistrationModal(false)}>Cancel
                            </button>
                        </div>

                        {errorMessage && <span className='text-danger'>{errorMessage}</span>}
                    </Form>
                </Spin>
            )}
        </Formik>
    );

    const registrationModal = <Modal
        open={openRegistrationModal}
        onClose={() => {
            setOpenRegistrationModal(false);
            setErrorMessage('');
        }}
    >
        <Box sx={{...style}}>
            <h2>Registration</h2>

            {registrationForm}
        </Box>
    </Modal>

    const logout = () => {
        delete axios.defaults.headers.common["Authorization"];
        localStorage.removeItem('token');

        dispatch({
            type: AuthReducerActionType.LOGOUT_USER,
            payload: null
        });

        navigate("/");
    }

    const {user} = auth;

    return (
        <>
            {loginModal}
            {registrationModal}

            <header className={`bg-dark ${styles.header}`}>
                <Link className="navbar-brand" aria-current="page" to={"/"}>
                    <h2 className="text-white">Lite Site</h2>
                </Link>

                <nav>
                    <ul className={`${styles.reset} ${styles.navList}`}>
                        {auth.isAuth
                            ?
                            <>
                                <li>
                                    <ul className={`${styles.reset} ${styles.navList} me-4`}>
                                        <Link className="navbar-brand" aria-current="page" to={"/basket"}>
                                            <button className="btn btn-info me-2 d-flex align-items-center">
                                                <span className="me-2">Basket</span>
                                                <ShoppingCartOutlined/>
                                            </button>
                                        </Link>
                                    </ul>
                                    <ul className={`${styles.reset} ${styles.navList} me-4`}>
                                        <Link className="navbar-brand" aria-current="page" to={"/orders"}>
                                            <button className="btn btn-info me-2 d-flex align-items-center">
                                                <span className="me-2">Orders</span>
                                                <ShoppingOutlined/>
                                            </button>
                                        </Link>
                                    </ul>
                                </li>
                                <li>
                                    <img className={`${styles.photo} me-2`} src={`${imagesDir}${user?.photo}`}
                                         alt='user photo'/>
                                </li>
                                <li><span className={`${styles.name} me-2`}>{user?.email}</span></li>
                                <li>
                                    <button className="btn btn-light" onClick={logout}>Logout</button>
                                </li>
                            </>
                            :
                            <>
                                <li>
                                    <button className="btn btn-light me-2"
                                            onClick={() => setOpenLoginModal(true)}>Login
                                    </button>
                                </li>
                                <li>
                                    <button className="btn btn-light"
                                            onClick={() => setOpenRegistrationModal(true)}>Registration
                                    </button>
                                </li>
                            </>}
                    </ul>
                </nav>
            </header>
        </>
    );
}

export default Header