# API plugin for nopCommerce v4.3

This plugin provides a RESTful API for managing resources in nopCommerce.

## Installation

- Download the code Api plugin v4.3

- Open file .zip and copy folder Nop.Plugin.Api to path -> {your NopCommerce solution}\Plugins

- Go to visual studio from tab ( View -> Solution Explorer )

- Right click on Plugins Folder -> Add -> Existing Project... -> Open file from path {your NopCommerce solution}\Plugins\Nop.Plugin.Api\Nop.Plugin.Api.csproj

- From Solution Explorer , right click on Nop.Plugin.Api and click on Rebuild.

- Now, press ctrl + F5 to start without debugging.

- Now, you can access the plugin UI in the admin -> Configuration -> Local plugins, then install Web Api plugin.



## What is a RESTful API?


HTTP requests are often the way that you interact with a RESTful API.
A client makes an HTTP request to a server and the server responds with an HTTP response.

In a HTTP request, you need to define the type of action that you want to perform against a resource. There are four primary actions associated with any HTTP request (commonly referred to as CRUD):

**POST** (Create)

**GET** (Retrieve)

**PUT** (Update)

**DELETE** (Delete)

A resource is a data object that can be accessed via an HTTP request. The API allows you to “access your nopCommerce site’s data (resources) through an easy-to-use HTTP REST API”. In the case of the most recent version of the API (nopCommerce version 3.80), the resources include the following 7 nopCommerce objects:

[**Customers**](Customers.md)

[**Products**](Products.md)

[**ProductReviews**](ProductReviews.md)

[**Categories**](Categories.md)

[**ProductCategoryMappings**](ProductCategoryMappings.md)

[**Orders**](Orders.md)

[**OrderItems**](OrderItems.md)

[**ShoppingCartItems**](ShoppingCartItems.md)

With the nopCommerce API, you can perform any of the four CRUD actions against any of your nopCommerce site’s resources listed above. For example, you can use the API to create a product, retrieve a product, update a product or delete a product associated with your nopCommerce website.

## What about security?

The API plugin currently supports generator token grant type flow. So in order to access the resource endpoints you need to provide a valid AccessToken.


[**Token**](Token.md)

I hope I helped you :)