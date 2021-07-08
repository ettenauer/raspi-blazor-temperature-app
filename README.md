# raspi-blazor-temperature-app
The project is part of proof of concept system to verify following technologies:
* Edge to Cloud/K8s (with Raspi)
* Services hosted in K8s using Helm
* Services hosted in AKS using Helm
* Istio with K8s
* Blazor WebAssembly
* Azure Identity 
* Inner Dev Loop with Docker-Compose

![System Design](https://github.com/ettenauer/raspi-blazor-temperature-app/blob/master/images/SystemDesign.PNG)

# Prerequisites
* Azure account to configure Azure Identity (Scope and AppRole)
* K8s cluster setup

# Inner Dev Loop with Docker-Compose
* open powershell/cmd
* navigate to docker-compose folder in repo
* docker-compose up

# Run on Docker Desktop K8s with Helm
## Cluster Setup
* install Docker Destop and activate K8s
* kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.47.0/deploy/static/provider/cloud/deploy.yaml
* create secret for image repository:
	* kubectl create secret docker-registry private-docker-registry-cred --docker-server=https://ghcr.io --docker-username=<USERNAME> --docker-password=<PASSWORD> --namespace=default -o yaml
* create secret self-signed TLS certificate for ingress (CN -> *.local):
	* openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout tls_self.key -out tls_self.crt -days 365 -subj "/CN=*.local" -days 365
	* kubectl create secret tls local-tls --cert=tls_self.crt --key=tls_self.key+
* create secrets for azure file share and deamon idenity and update it in https://github.com/ettenauer/raspi-blazor-temperature-app/blob/master/k8s/templates/importfileSecret.yaml
* update secrets for db https://github.com/ettenauer/raspi-blazor-temperature-app/blob/master/k8s/templates/dbSecret.yaml
* install helm

## Run Application
* helm install local ./helm 
* app should accessible via ingress -> https://raspi.local/
* helm uninstall local

## Run Application with service mesh ISTIO
* download istio https://istio.io/downloadIstio
* update PATH and include istioctl.exe location
* istioctl install --set profile=demo -y
* kubectl label namespace default istio-injection=enabled -> required otherwise sidecars are not injected automatically 
* kubectl apply -f ./istio-k8s/addons -> start addon like kiali or jaeger
* kubectl create -n istio-system secret tls local-tls --key=tls_self.key --cert=tls_self.crt -> create cert for raspi.local with openssl
* helm install local .\istio-k8s
* kubectl get pods -> check if pods have two container 
* istioctl dashboard kiali -> load dashboard to view system
* browse https://raspi.local

# Run Edge Application on Raspberry Pi

## Steps
1. set up raspberry with ansible, please check out instruction here https://github.com/ettenauer/raspi-ansible
2. deploy playbook https://github.com/ettenauer/raspi-ansible/blob/main/DHT22.yaml on you raspberry pi
3. connect via ssh to your raspberry pi
4. use docker or docker compose to run image ghcr.io/ettenauer/raspi-blazor-temperature-app-edge:master

## docker
* docker pull ghcr.io/ettenauer/raspi-blazor-temperature-app-edge:master
* docker run --network="host" ghcr.io/ettenauer/raspi-blazor-temperature-app-edge:master

## docker-compose
* docker-compose up -> yml can found here https://github.com/ettenauer/raspi-blazor-temperature-app/blob/master/source/Edge/docker-compose.yml
