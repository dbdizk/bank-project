import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

class AccountComponent extends React.Component{
  constructor(props){
     super(props);

     this.state={
       account:[]
     };
   }
   componentDidMount() {
     fetch("https://localhost:7055/api/account")
       .then(res => res.json())
       .then(
         (result) => {
           this.setState({
             account: result
           });
         }
       );
   }

   render(){
     return (
       <div>
         <h2>Account Data</h2>
         <table>
           <thead>
             <tr>
               <th>ID</th>
               <th>Balance</th>
             </tr>
           </thead>
           <tbody>
             {this.state.account.map(acc=>(
               <tr key={acc.id}>
                 <td>{acc.id}</td>
                 <td>{acc.balance}</td>
               </tr>))}
           </tbody>
         </table>
       </div>
     );
   }
 }


const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <App />
    <AccountComponent />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
