db = db.getSiblingDB('Message');
db.createCollection('Errors');
db.getCollection('Errors').insertMany([
    { "key": "ProductNameNullOrEmpty", "message": "O nome do produto não pode ser vazio ou nulo." },
    { "key": "ProductDescriptionNullOrEmpty", "message": "A descrição do produto não pode ser vazio ou nulo." },
    { "key": "ProductValueLessThanEqualToZero", "message": "O valor do produto não pode ser menor ou igual a zero." },
    { "key": "ProductQuantityLessThanEqualToZero", "message": "A quantidade do produto não pode ser menor ou igual a zero." },
    { "key": "ProductImageNullOrEmpty", "message": "A imagem do produto não pode ser vazio ou nulo." },
    { "key": "ProductQuantityDebitedLessThanOrEqualToZero", "message": "A quantidade debitada do produto não pode ser menor ou igual a zero." },
    { "key": "ProductQuantityDebitedLargerThanAvailable", "message": "A quantidade debitada do produto não pode ser maior que a quantidade disponível" },
    { "key": "ProductQuantityAddedLessThanOrEqualToZero", "message": "A quantidade adicionada do produto não pode ser menor ou igual a zero." },
    { "key": "ProductInvalidId", "message": "O id do produto não pode ser inválido." },
    { "key": "ProductInvalidCategoryId", "message": "O id da categoria do produto não pode ser inválido." },
    { "key": "ProductCategoryNotFound", "message": "A categoria do produto não foi encontrada." },
    { "key": "ProductAnErrorOccorred", "message": "Ocorreu um erro ao cadastrar ou atualizar o produto." },
    { "key": "ProductRegisterDuplicity", "message": "Não é possível cadastrar ou atualizar um produto duplicado." },
    { "key": "CategoryCodeLessThanEqualToZero", "message": "O código da categoria não pode ser menor ou igual a zero." },
    { "key": "CategoryNameNullOrEmpty", "message": "O nome da categoria não pode ser vazio ou nulo." },
    { "key": "CategoryDescriptionNullOrEmpty", "message": "A descrição da categoria não pode ser vazio ou nulo." },
    { "key": "CategoryInvalidId", "message": "O id da categoria não pode ser inválido." },
    { "key": "CategoryAnErrorOccorred", "message": "Ocorreu um erro ao cadastrar ou atualizar a categoria." },
    { "key": "CategoryRegisterDuplicity", "message": "Não é possível cadastrar ou atualizar uma categoria duplicada." }
])

db = db.getSiblingDB('Catalog');
db.createCollection('Categories');
db.createCollection('Products');

db = db.getSiblingDB('Cart');
db.createCollection('Carts');