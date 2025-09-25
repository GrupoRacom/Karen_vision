# Política de Seguridad - KarenVision

## 🔒 Reportar Vulnerabilidades de Seguridad

Si descubres una vulnerabilidad de seguridad en KarenVision, por favor repórtala de manera responsable.

### 📧 Contacto para Seguridad

**NO** reportes vulnerabilidades de seguridad a través de issues públicos de GitHub.

En su lugar, envía un email a: **security@karenvision.com**

### 📝 Información a Incluir

Por favor incluye la siguiente información:
- Descripción detallada de la vulnerabilidad
- Pasos para reproducir el problema
- Versión afectada del software
- Posible impacto de la vulnerabilidad
- Sugerencias para la corrección (si las tienes)

### ⏱️ Tiempo de Respuesta

- **Confirmación inicial**: 48 horas
- **Evaluación completa**: 7 días
- **Corrección**: Dependiendo de la severidad (1-30 días)

### 🛡️ Versiones Soportadas

| Versión | Soporte de Seguridad |
| ------- | ------------------- |
| 1.0.x   | ✅ Sí |
| < 1.0   | ❌ No |

### 🚨 Clasificación de Severidad

#### 🔥 Crítica
- Ejecución remota de código
- Acceso no autorizado a datos sensibles
- Escalada de privilegios

#### ⚠️ Alta  
- Denegación de servicio
- Bypass de autenticación
- Inyección SQL

#### 📝 Media
- Divulgación de información
- Cross-Site Scripting (XSS)
- CSRF

#### ℹ️ Baja
- Problemas de configuración menores
- Divulgación mínima de información

### 🏆 Reconocimientos

Los investigadores que reporten vulnerabilidades válidas serán reconocidos en:
- Hall of Fame de seguridad
- Notas de versión
- Documentación de seguridad

### 🔐 Mejores Prácticas de Seguridad

#### Para Usuarios
- Mantén la aplicación actualizada
- Usa contraseñas seguras para MySQL
- No ejecutes la aplicación con privilegios administrativos
- Realiza backups regulares de la base de datos

#### Para Desarrolladores
- Valida todas las entradas de usuario
- Usa consultas parametrizadas (Entity Framework)
- Implementa manejo seguro de errores
- Audita dependencias regularmente

### 📚 Recursos de Seguridad

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [.NET Security Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- [MySQL Security Guidelines](https://dev.mysql.com/doc/refman/8.0/en/security.html)

---

**Nota**: Esta política se actualiza regularmente. Última actualización: Septiembre 2025