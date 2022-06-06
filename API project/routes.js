const express = require("express"),
    router = express.Router(),
    dbCtrl = require("./db-controller");

router.get('/', dbCtrl.test);
router.post('/find/:collection/:id', dbCtrl.findOne);
router.post("/find/:collection", dbCtrl.findMany);
router.post("/insert/:collection",dbCtrl.insertOne);
router.delete("/delete/:collection",dbCtrl.deleteOne);
router.put("/update/:collection/:id",dbCtrl.updateOne);

module.exports = router;


