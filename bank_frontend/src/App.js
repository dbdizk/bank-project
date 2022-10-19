import React, { useEffect, useState } from 'react';
import Axios from 'axios';
import Button from 'react-bootstrap/Button';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from './App.css';

function App() {

  const [usernameReg, setUsernameReg] = useState('');
  const [passwordReg, setPasswordReg] = useState('');

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const [loginStatus, setLoginStatus] = useState(false);

  const [accountList, setAccountList] = useState([]);

  Axios.defaults.withCredentials = true;

  const register = () => {
    Axios.post('http://localhost:3306/register', {username: usernameReg, password: passwordReg,
  }).then((response) => {
    console.log(response);
  });
  };


  const login = () => {
    Axios.post('http://localhost:3306/login', {username: username, password: password,
  }).then((response) => {
    console.log(response);
    if (!response.data.auth) {
      setLoginStatus(false);
    } else {
      console.log(response.data);
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("logged", response.data.result[0].iduser);
      setLoginStatus(true);
    }
    
  });
  };

  const getAccounts = () => {
    Axios.get("http://localhost:3306/myAccounts").then((response) => {
      setAccountList(response.data);
    });
  };

  const userAuthenticated = () => {
    Axios.get("http://localhost:3306/isUserAuth", {
      headers: {
        "x-access-token": localStorage.getItem("token"),
      }
    }).then((response)=> {
      console.log(response);
    }
    );
  };

  useEffect(()=> {
    Axios.get("http://localhost:3306/login").then((response) => {
      console.log(response);
    if (response.data.loggedIn == true) {    
    setLoginStatus(response.data.user[0].username);
    }
    });
  }, []);


  return (
    <div className="App">
      <div className="registration">
        <h1>Registration</h1>
        <label>Username</label>
        <input type="text" onChange={(e) => {
          setUsernameReg(e.target.value);
        }}
        />
        <br/>
        <label>Password</label>
        <input type="text" onChange={(e) => {
          setPasswordReg(e.target.value);
        }}/>
        <br/>
        <button onClick={register}>Register</button>
      </div>
      
      
      
      <div className="login">
        <h1>Login</h1>
        <input type="text" placeholder="Username..." onChange={(e) => {
          setUsername(e.target.value);
        }}/>
        <input type="password" placeholder="Password..." onChange={(e) => {
          setPassword(e.target.value);
        }}/>
        <button onClick={login}>Login</button>
      </div>

      {loginStatus && (
        <button onClick={userAuthenticated}>Check if authenticated</button>
      )}
      {/* <div className="accounts">
        <button onClick={getAccounts}>Show Accounts</button>
        <h3>
        </h3>
        {accountList.map((val) => {
          return (
            
            <div className="accounts">
              <div>
                <h3>Account ID: {val.idaccount}</h3>
                <h3>Balance: {val.balance}</h3>
              </div>
             
            </div>
          );
        })}
      </div> */}
    </div>

  );
}

export default App;
