

# Products Module

## Objective

The Products module is responsible for managing all products sold by the company.

It is the foundation of the ERP, since Inventory, Purchasing, Sales, Finance and Reports depend directly on product information.

---

# Main Business Processes

- Register a new product
- Update product information
- Change sale price
- Change cost price
- Activate a product
- Deactivate a product
- Associate a category
- Associate a brand
- Search products
- List products
- View product details

---

# Business Rules

- Every product must have a unique SKU.
- Barcode is optional.
- Every product belongs to exactly one category.
- Every product belongs to exactly one brand.
- Products are never physically deleted.
- Products can be deactivated.
- Sale price cannot be less than zero.
- Cost price cannot be less than zero.
- Products cannot be sold when inactive.

---

# Future Features

- Price history
- Product images
- Multiple barcodes
- Product kits
- Product variations
- Multiple units of measure