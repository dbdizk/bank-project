const express = require("express");
const mysql = require("mysql");
const cors = require("cors");
const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const session = require('express-session');
const bcrypt = require('bcrypt');
const saltRounds = 10;

const jwt = require('jsonwebtoken');

const app = express();

app.use(express.json());
app.use(cors({
    origin: ["http://localhost:3000"],
    methods: ["GET", "POST"],
    credentials: true
}));
app.use(cookieParser());
app.use(bodyParser.urlencoded({extended: true}));

app.use(session({
    key: "userId",
    secret: "bank",
    resave: false,
    saveUninitialized: false,
    cookie: {
        expires: 60*60*24
    },
})
);

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

const verifyJWT = (req, res, next) => {
    const token = req.headers["x-access-token"];

    if (!token) {
        res.send("Need a token!");
    } else {
        jwt.verify(token, "bankSecret", (err, decoded) =>{
            if (err) {
                res.json({auth: false, message: "You failed to authenticate"});
            } else {
                req.userId = decoded.id;
                next();
            }
        });
    }
};

app.get("/isUserAuth", verifyJWT, (req,res) => {
    res.send("You are authenticated!")
})

app.get("/login", (req, res) => {
    if(req.session.user) {
        res.send({loggedIn: true, user: req.session.user});
    } else {
        res.send({loggedIn: false});
    }
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
                    const id = result[0].id;
                    const token = jwt.sign({id}, "bankSecret", {
                        expiresIn: 300,
                    });
                    req.session.user = result;
                    res.json({auth: true, token: token, result: result});
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