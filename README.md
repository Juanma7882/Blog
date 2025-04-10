📝 Blog de Notas - Backend
Este es el backend de una aplicación de Blog de Notas, desarrollada con .NET Core y utilizando SQL Server como sistema de base de datos. La comunicación con la base de datos se gestiona mediante Entity Framework Core (EF Core) como ORM, lo que permite una interacción fluida y segura con las entidades del dominio.

🔧 Características principales:
API RESTful estructurada y escalable.

Gestión de blogs con sus atributos: título, contenido, descripción, enlace, y fecha de publicación.

Asociación de múltiples etiquetas a cada blog, con relaciones muchos a muchos implementadas mediante una tabla intermedia (BlogEtiqueta).

Validaciones personalizadas y control de errores.

Arquitectura organizada por capas (DTOs, Entidades, Mapeadores, Repositorios).

Implementación genérica de operaciones CRUD.

Preparado para escalar con funcionalidades como roles de usuario, autenticación y autorización.
