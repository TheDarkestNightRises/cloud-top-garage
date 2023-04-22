#------------User Service--------------
# Deployments
kubectl delete deployment users-depl
kubectl delete deployment users-mssql-depl

# Services
kubectl delete service users-clusterip-srv
kubectl delete service users-mssql-clusterip-srv
kubectl delete service users-mssql-loadbalancer