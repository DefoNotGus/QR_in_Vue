# Vue QR App

## Run with Docker

1. Make sure Docker is installed.
2. Clone the repository and navigate to the folder.
3. Run:

```bash
docker-compose up --build
```

FE: localhost:8080
BE: localhost:5000

The backend persists scanned values in `data/items.json`. When running via
Docker this file is mapped to the `data/` directory next to the
`docker-compose.yml` so values survive container restarts.
