const res = require('express/lib/response');
const { MongoClient, ServerApiVersion } = require('mongodb');
const client = new MongoClient(process.env.dbURI, { useNewUrlParser: true, >

exports.findMany = function(req, res){
    var collection = req.params.collection;
    client.db("ATGPmongodb").collection(collection).find({}, function(err, >
        if(err){
            res.status(400).json(err);
        }
        res.json(results);
    });
};



