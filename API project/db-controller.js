const res = require('express/lib/response');
const { MongoClient} = require('mongodb');
const dotenv=require("dotenv");
const client =  MongoClient
const ObjectId = require("mongodb").ObjectID;

exports.findOne = async function(req, res,cb){
    var collection = req.params.collection;
        console.log(req.body);
 try {
    await client.connect(process.env.dbURI,function(err,db){
  if(err) throw err;
   var dbo=db.db("ATGPmongodb");
   var query = {_id: ObjectId(req.params.id)};
 dbo.collection(req.params.collection).find(query).toArray(function(err,res>
   if (err) throw err;
 console.log (results);
db.close();
res.json(results);
return cb(null, results);
}
 );
});
 
} catch (e) {
    console.error(e);
}
};

exports.findMany = async function(req, res,cb){
        console.log(req);
    var collection = req.params.collection;
        console.log(req.body);
 try {
    await client.connect(process.env.dbURI,function(err,db){
  if(err) throw err;
   var dbo=db.db("ATGPmongodb");
   var query = req.body;
 dbo.collection(req.params.collection).find(query).toArray(function(err,res>
   if (err) throw err;
 console.log (results);
db.close();
res.json(results);
return cb(null, results);
}
 );
});
 
} catch (e) {
    console.error(e);
}
};

exports.insertOne = async function(req, res,cb){
    var collection = req.params.collection;
    console.log(req.body);
 try {
    await client.connect(process.env.dbURI,function(err,db){
  if(err) throw err;
   var dbo=db.db("ATGPmongodb");
   var object = req.body;
   var objId = req.body._id;
   console.log(objId);
   object["_id"] = ObjectId(objId);
 dbo.collection(req.params.collection).insertOne(object, function(err,resul>
   if (err) throw err;
 var success = "inserted "+ object ;
db.close();
res.json(success);
return cb(null, success);
}
 );
});
 
} catch (e) {
    console.error(e);
}
};

exports.deleteOne = async function(req, res,cb){
    var collection = req.params.collection;
 try {
    await client.connect(process.env.dbURI,function(err,db){
  if(err) throw err;
   var dbo=db.db("ATGPmongodb");
   var query = {_id: ObjectId(req.body.id)};
 dbo.collection(req.params.collection).deleteOne(query, function(err,result>
   if (err) throw err;
 var success = "deleted "+ results ;
db.close();
res.json(success);
return cb(null, success);
}
 );
});

} catch (e) {
    console.error(e);
}
};

exports.updateOne = async function(req, res,cb){
    var collection = req.params.collection;
 try {
    await client.connect(process.env.dbURI,function(err,db){
  if(err) throw err;
   var dbo=db.db("ATGPmongodb");
   var newValues  = req.body;
   var query = {_id: ObjectId( req.params.id)};
 dbo.collection(req.params.collection).updateOne(query,newValues , function>
   if (err) throw err;
 var success = "updated "+ results ;
db.close();
res.json(success);
return cb(null, success);
}
 );
});

} catch (e) {
    console.error(e);
}
};



exports.test =  function(req, res) {
    res.json({
        "find one": {
                "url": "/find/collectionName/objectId",
                "method": "post",
                "body": "none",
                "comment": "objectId on url in string format"
        },
        "find many": {
                "url": "/find/collectionName",
                "method": "post",
                "body": "parameters in bson format ex: {'region': 'EU'}",
                "comment": "If no parameters are passed, all documents from>
        },
        "insert one": {
                "url": "/insert/collectionName",
                "method": "post",
                "body": "object to be inserted in bson format ex: {'id': ob>
                "comment": "object id to be passed as string"
        },
        "delete  one": {
                "url": "/delete/collectionName",
                "method": "delete",
                "body": "parameters in bson format ex: {'id': objectId}",
                "comment": "object id to be passed as string"
        },
        "update one": {
                "url": "/update/collectionName/objectId",
                "method": "put",
                "body": "update set in bson format ex: {'$set': {'region': >
                "comment": "objectId on url in string format"
        }
    });
}