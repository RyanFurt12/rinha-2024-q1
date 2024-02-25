CREATE TABLE clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    limite INT NOT NULL,
    valor INT NOT NULL
);

CREATE TABLE transacoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cliente_id INT NOT NULL,
    valor INT NOT NULL,
    tipo CHAR(1) NOT NULL,
    descricao VARCHAR(10) NOT NULL,
    criada_em DATETIME,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

INSERT INTO clientes (limite, valor)
SELECT * FROM (
    SELECT 100000 AS limite, 0 AS valor
    UNION ALL
    SELECT 80000, 0
    UNION ALL
    SELECT 1000000, 0
    UNION ALL
    SELECT 10000000, 0
    UNION ALL
    SELECT 500000, 0
) AS tmp
WHERE NOT EXISTS (
    SELECT id FROM clientes LIMIT 1
);