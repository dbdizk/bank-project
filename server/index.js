const express = require("express");
const mysql = require("mysql");
const cors = require("cors");

const bcrypt = require('bcrypt');
const saltRounds = 10;

const app = express();

app.use(express.json());
app.use(cors());

const db = mysql.createConnection({
    user: "root",
    host: "localhost",
    password: "pass",
    database: "bank"
});

app.post('/register', (req, res)=> {
    
    
    const username = req.body.username
    const password = req.body.password

    bcrypt.hash(password,saltRounds, (err, hash) => {

        if (err){
            console.log(err)
        }

        db.query("INSERT INTO user (username, password) VALUES (?,?)",
        [username, hash],
        (err, result) => {
           console.log(err);
        }
        );
    })
    
    
});

app.post('/login', (req,res)=> {

    const username = req.body.username
    const password = req.body.password


    db.query("SELECT * FROM user WHERE username = ?;",
    username,
    (err, result) => {
       if (err) {
        res.send({err: err});
       }


        if (result.length > 0) {
            bcrypt.compare(password, result[0].password, (error, response) => {
                if (response) {
                    res.send(result);
                } else {
                    res.send({message: "Wrong username/password!"});
                }
        });
        } else {
            res.send({message: "User doesn't exist!"});
        }
       
    }
    );
});


app.listen(3306, () => {
    console.log("running server");
});