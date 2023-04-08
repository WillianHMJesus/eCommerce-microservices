db = db.getSiblingDB('Catalog');
db.createCollection('Categories');
db.createCollection('Products');

db = db.getSiblingDB('Cart');
db.createCollection('Carts');