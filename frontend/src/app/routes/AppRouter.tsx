
import { useEffect, useMemo, useState, type FormEvent } from "react";
import { NavLink, Route, Routes } from "react-router-dom";
import { httpClient } from "@/core/api/httpClient";

type Category = { id: string; name: string };
type Brand = { id: string; name: string };
type Product = {
  id: string; name: string; description?: string; sku: string; barcode?: string;
  categoryId: string; brandId?: string; category?: string; brand?: string;
  costPrice: number; salePrice: number; minimumStock: number; stock: number; isActive: boolean;
};
type InventoryItem = { productId: string; product: string; sku: string; quantity: number; minimumStock: number; status: string };
type Supplier = { id: string; name: string };
type Customer = { id: string; name: string };
type Dashboard = { salesToday: number; totalProducts: number; lowStock: number; recentSales: Array<{ id: string; total: number; paymentMethod: string; createdAt: string }> };

const money = (value: number) => value.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

function Layout({ children }: { children: React.ReactNode }) {
  const links = [
    ["/", "Dashboard"],
    ["/produtos", "Produtos"],
    ["/estoque", "Estoque"],
    ["/compras", "Compras"],
    ["/vendas", "Vendas"],
    ["/cadastros", "Cadastros"],
  ];
  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="brand"><span>MERKA</span><small>ERP para pequenos mercados</small></div>
        <nav>{links.map(([to, label]) => <NavLink key={to} to={to} end={to === "/"}>{label}</NavLink>)}</nav>
        <div className="sidebar-footer">Sistema MVP • v1.0</div>
      </aside>
      <main className="content">{children}</main>
    </div>
  );
}

function DashboardPage() {
  const [data, setData] = useState<Dashboard | null>(null);
  useEffect(() => { void httpClient.get<Dashboard>("/dashboard").then(r => setData(r.data)); }, []);
  return <Page title="Visão geral" subtitle="Acompanhe os principais números do seu mercado.">
    <div className="cards">
      <Card label="Vendas de hoje" value={data ? money(data.salesToday) : "Carregando..."} />
      <Card label="Produtos ativos" value={data?.totalProducts ?? "—"} />
      <Card label="Estoque baixo" value={data?.lowStock ?? "—"} danger />
    </div>
    <section className="panel"><h2>Últimas vendas</h2>
      {!data?.recentSales.length ? <Empty text="Nenhuma venda registrada ainda." /> :
        <table><thead><tr><th>Data</th><th>Pagamento</th><th>Total</th></tr></thead><tbody>
          {data.recentSales.map(s => <tr key={s.id}><td>{new Date(s.createdAt).toLocaleString("pt-BR")}</td><td>{s.paymentMethod}</td><td>{money(s.total)}</td></tr>)}
        </tbody></table>}
    </section>
  </Page>;
}

function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [search, setSearch] = useState("");
  const [open, setOpen] = useState(false);
  const empty = { name: "", description: "", sku: "", barcode: "", categoryId: "", brandId: "", costPrice: 0, salePrice: 0, minimumStock: 0, initialStock: 0 };
  const [form, setForm] = useState(empty);
  const load = async () => {
    const [p, c, b] = await Promise.all([httpClient.get<Product[]>("/products"), httpClient.get<Category[]>("/categories"), httpClient.get<Brand[]>("/brands")]);
    setProducts(p.data); setCategories(c.data); setBrands(b.data);
  };
  useEffect(() => { void load(); }, []);
  const filtered = useMemo(() => products.filter(p => `${p.name} ${p.sku} ${p.barcode ?? ""}`.toLowerCase().includes(search.toLowerCase())), [products, search]);
  const submit = async (e: FormEvent) => {
    e.preventDefault();
    await httpClient.post("/products", { ...form, brandId: form.brandId || null, costPrice: Number(form.costPrice), salePrice: Number(form.salePrice), minimumStock: Number(form.minimumStock), initialStock: Number(form.initialStock) });
    setForm(empty); setOpen(false); await load();
  };
  return <Page title="Produtos" subtitle="Cadastre, consulte e gerencie o catálogo do mercado." action={<button onClick={() => setOpen(true)}>+ Novo produto</button>}>
    <div className="toolbar"><input placeholder="Buscar por nome, SKU ou código de barras" value={search} onChange={e => setSearch(e.target.value)} /></div>
    <section className="panel"><table><thead><tr><th>Produto</th><th>SKU</th><th>Categoria</th><th>Estoque</th><th>Custo</th><th>Venda</th><th>Status</th></tr></thead><tbody>
      {filtered.map(p => <tr key={p.id}><td><strong>{p.name}</strong><small>{p.brand ?? "Sem marca"}</small></td><td>{p.sku}</td><td>{p.category}</td><td className={p.stock <= p.minimumStock ? "danger-text" : ""}>{p.stock}</td><td>{money(p.costPrice)}</td><td>{money(p.salePrice)}</td><td><button className={p.isActive ? "tag success" : "tag"} onClick={async () => { await httpClient.patch(`/products/${p.id}/status`); await load(); }}>{p.isActive ? "Ativo" : "Inativo"}</button></td></tr>)}
    </tbody></table>{!filtered.length && <Empty text="Nenhum produto encontrado." />}</section>
    {open && <Modal title="Novo produto" onClose={() => setOpen(false)}><form className="form-grid" onSubmit={submit}>
      <input required placeholder="Nome do produto" value={form.name} onChange={e => setForm({...form,name:e.target.value})}/>
      <input required placeholder="SKU" value={form.sku} onChange={e => setForm({...form,sku:e.target.value})}/>
      <input placeholder="Código de barras" value={form.barcode} onChange={e => setForm({...form,barcode:e.target.value})}/>
      <select required value={form.categoryId} onChange={e => setForm({...form,categoryId:e.target.value})}><option value="">Selecione a categoria</option>{categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}</select>
      <select value={form.brandId} onChange={e => setForm({...form,brandId:e.target.value})}><option value="">Sem marca</option>{brands.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}</select>
      <input type="number" step="0.01" min="0" placeholder="Preço de custo" value={form.costPrice} onChange={e => setForm({...form,costPrice:Number(e.target.value)})}/>
      <input type="number" step="0.01" min="0" placeholder="Preço de venda" value={form.salePrice} onChange={e => setForm({...form,salePrice:Number(e.target.value)})}/>
      <input type="number" min="0" placeholder="Estoque inicial" value={form.initialStock} onChange={e => setForm({...form,initialStock:Number(e.target.value)})}/>
      <input type="number" min="0" placeholder="Estoque mínimo" value={form.minimumStock} onChange={e => setForm({...form,minimumStock:Number(e.target.value)})}/>
      <textarea placeholder="Descrição" value={form.description} onChange={e => setForm({...form,description:e.target.value})}/>
      <div className="form-actions"><button type="button" className="secondary" onClick={() => setOpen(false)}>Cancelar</button><button type="submit">Salvar produto</button></div>
    </form></Modal>}
  </Page>;
}

function InventoryPage() {
  const [items, setItems] = useState<InventoryItem[]>([]);
  const load = async () => setItems((await httpClient.get<InventoryItem[]>("/inventory")).data);
  useEffect(() => { void load(); }, []);
  return <Page title="Estoque" subtitle="Acompanhe o saldo e ajuste quantidades manualmente.">
    <section className="panel"><table><thead><tr><th>Produto</th><th>SKU</th><th>Quantidade</th><th>Mínimo</th><th>Status</th><th>Ajuste</th></tr></thead><tbody>
      {items.map(i => <tr key={i.productId}><td>{i.product}</td><td>{i.sku}</td><td className={i.status === "Baixo" ? "danger-text" : ""}>{i.quantity}</td><td>{i.minimumStock}</td><td><span className={i.status === "Baixo" ? "tag danger" : "tag success"}>{i.status}</span></td><td><button className="secondary" onClick={async () => { const raw = prompt("Informe o ajuste. Ex.: 10 para entrada ou -3 para saída"); const value = Number(raw); if (Number.isNaN(value) || value === 0) return; await httpClient.post(`/inventory/${i.productId}/adjust`, null, { params: { quantity: value } }); await load(); }}>Ajustar</button></td></tr>)}
    </tbody></table>{!items.length && <Empty text="Cadastre produtos para começar a controlar o estoque." />}</section>
  </Page>;
}

function TransactionsPage({ type }: { type: "sales" | "purchases" }) {
  const isSale = type === "sales";
  const [products, setProducts] = useState<Product[]>([]);
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [partyId, setPartyId] = useState("");
  const [payment, setPayment] = useState("Dinheiro");
  const [lines, setLines] = useState<Array<{ productId: string; quantity: number; unitPrice: number }>>([]);
  const [history, setHistory] = useState<Array<{ id: string; total: number; createdAt: string }>>([]);
  const [selected, setSelected] = useState("");
  const load = async () => {
    const [p, h] = await Promise.all([httpClient.get<Product[]>("/products"), httpClient.get<Array<{ id: string; total: number; createdAt: string }>>(`/${type}`)]);
    setProducts(p.data.filter(x => isSale ? x.isActive && x.stock > 0 : x.isActive)); setHistory(h.data);
    if (isSale) setCustomers((await httpClient.get<Customer[]>("/customers")).data);
    else setSuppliers((await httpClient.get<Supplier[]>("/suppliers")).data);
  };
  useEffect(() => { void load(); }, [isSale]);
  const addLine = () => {
    const product = products.find(p => p.id === selected);
    if (!product) return;
    setLines([...lines, { productId: product.id, quantity: 1, unitPrice: isSale ? product.salePrice : product.costPrice }]);
    setSelected("");
  };
  const total = lines.reduce((sum, line) => sum + line.quantity * line.unitPrice, 0);
  const save = async () => {
    if (!lines.length) return alert("Adicione ao menos um item.");
    if (!partyId) return alert(isSale ? "Selecione um cliente." : "Selecione um fornecedor.");
    const payload = isSale ? { customerId: partyId, paymentMethod: payment, items: lines } : { supplierId: partyId, items: lines };
    await httpClient.post(`/${type}`, payload); setLines([]); setPartyId(""); await load();
  };
  const party = isSale ? customers : suppliers;
  return <Page title={isSale ? "Vendas" : "Compras"} subtitle={isSale ? "Registre vendas e baixe o estoque automaticamente." : "Registre compras e dê entrada no estoque automaticamente."}>
    <div className="two-columns"><section className="panel transaction"><h2>Nova {isSale ? "venda" : "compra"}</h2>
      <select value={partyId} onChange={e => setPartyId(e.target.value)}><option value="">Selecione {isSale ? "o cliente" : "o fornecedor"}</option>{party.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}</select>
      {isSale && <select value={payment} onChange={e => setPayment(e.target.value)}><option>Dinheiro</option><option>Pix</option><option>Cartão de débito</option><option>Cartão de crédito</option></select>}
      <div className="add-line"><select value={selected} onChange={e => setSelected(e.target.value)}><option value="">Selecione um produto</option>{products.map(p => <option key={p.id} value={p.id}>{p.name} — {money(isSale ? p.salePrice : p.costPrice)}</option>)}</select><button onClick={addLine}>Adicionar</button></div>
      {lines.map((l, index) => <div className="line" key={`${l.productId}-${index}`}><span>{products.find(p => p.id === l.productId)?.name}</span><input type="number" min="0.001" step="0.001" value={l.quantity} onChange={e => setLines(lines.map((x,i) => i===index ? {...x,quantity:Number(e.target.value)} : x))}/><input type="number" min="0" step="0.01" value={l.unitPrice} onChange={e => setLines(lines.map((x,i) => i===index ? {...x,unitPrice:Number(e.target.value)} : x))}/><button className="danger-button" onClick={() => setLines(lines.filter((_,i) => i!==index))}>×</button></div>)}
      <div className="total">Total: <strong>{money(total)}</strong></div><button onClick={save}>Finalizar {isSale ? "venda" : "compra"}</button>
    </section><section className="panel"><h2>Últimos lançamentos</h2>{history.slice(0,10).map(h => <div className="history" key={h.id}><span>{new Date(h.createdAt).toLocaleString("pt-BR")}</span><strong>{money(h.total)}</strong></div>)}{!history.length && <Empty text="Nenhum lançamento ainda." />}</section></div>
  </Page>;
}

function RecordsPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [name, setName] = useState("");
  const load = async () => {
    const [c,b,s,u] = await Promise.all([httpClient.get<Category[]>("/categories"),httpClient.get<Brand[]>("/brands"),httpClient.get<Supplier[]>("/suppliers"),httpClient.get<Customer[]>("/customers")]);
    setCategories(c.data);setBrands(b.data);setSuppliers(s.data);setCustomers(u.data);
  };
  useEffect(() => { void load(); }, []);
  const create = async (endpoint: string) => { if (!name.trim()) return; await httpClient.post(endpoint,{name}); setName(""); await load(); };
  const block = (title: string, items: Array<{id:string;name:string}>, endpoint: string) => <section className="panel"><h2>{title}</h2><div className="inline-form"><input value={name} onChange={e=>setName(e.target.value)} placeholder={`Novo ${title.slice(0,-1).toLowerCase()}`}/><button onClick={()=>create(endpoint)}>Adicionar</button></div>{items.map(x=><div className="history" key={x.id}>{x.name}</div>)}</section>;
  return <Page title="Cadastros" subtitle="Gerencie informações auxiliares do ERP."><div className="records-grid">{block("Categorias",categories,"/categories")}{block("Marcas",brands,"/brands")}{block("Fornecedores",suppliers,"/suppliers")}{block("Clientes",customers,"/customers")}</div></Page>;
}

function Page({ title, subtitle, action, children }: { title: string; subtitle: string; action?: React.ReactNode; children: React.ReactNode }) {
  return <><header className="page-header"><div><h1>{title}</h1><p>{subtitle}</p></div>{action}</header>{children}</>;
}
function Card({ label, value, danger }: { label: string; value: string | number; danger?: boolean }) { return <div className={`card ${danger ? "card-danger" : ""}`}><span>{label}</span><strong>{value}</strong></div>; }
function Empty({ text }: { text: string }) { return <div className="empty">{text}</div>; }
function Modal({ title, children, onClose }: { title: string; children: React.ReactNode; onClose: () => void }) { return <div className="modal-backdrop"><div className="modal"><div className="modal-header"><h2>{title}</h2><button className="close" onClick={onClose}>×</button></div>{children}</div></div>; }

export function AppRouter() {
  return <Layout><Routes>
    <Route path="/" element={<DashboardPage />} />
    <Route path="/produtos" element={<ProductsPage />} />
    <Route path="/estoque" element={<InventoryPage />} />
    <Route path="/compras" element={<TransactionsPage type="purchases" />} />
    <Route path="/vendas" element={<TransactionsPage type="sales" />} />
    <Route path="/cadastros" element={<RecordsPage />} />
  </Routes></Layout>;
}
