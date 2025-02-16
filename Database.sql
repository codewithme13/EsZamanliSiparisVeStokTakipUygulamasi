--
-- PostgreSQL database dump
--

-- Dumped from database version 14.13
-- Dumped by pg_dump version 16.4

-- Started on 2025-02-16 16:51:43

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 210 (class 1259 OID 139266)
-- Name: customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customers (
    customerid integer NOT NULL,
    customername character varying(100) NOT NULL,
    budget numeric(10,2) NOT NULL,
    customertype character varying(50) NOT NULL,
    totalspent numeric(10,2) DEFAULT 0
);


ALTER TABLE public.customers OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 139265)
-- Name: customers_customerid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customers_customerid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.customers_customerid_seq OWNER TO postgres;

--
-- TOC entry 3356 (class 0 OID 0)
-- Dependencies: 209
-- Name: customers_customerid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customers_customerid_seq OWNED BY public.customers.customerid;


--
-- TOC entry 216 (class 1259 OID 139300)
-- Name: logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.logs (
    logid integer NOT NULL,
    customerid integer NOT NULL,
    orderid integer NOT NULL,
    logdate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    logtype character varying(50) NOT NULL,
    logdetails character varying(255) NOT NULL,
    customertype character varying(50),
    CONSTRAINT logs_customertype_check CHECK (((customertype)::text = ANY ((ARRAY['Premium'::character varying, 'Standart'::character varying])::text[]))),
    CONSTRAINT logs_logdetails_check CHECK (((logdetails)::text = ANY ((ARRAY['Satın alma başarılı'::character varying, 'Ürün stoğu yetersiz'::character varying, 'Zaman aşımı'::character varying, 'Müşteri bakiyesi yetersiz'::character varying, 'Veritabanı Hatası'::character varying, 'Bütçe yetersiz'::character varying, 'Satın alma isteği başarılı'::character varying])::text[]))),
    CONSTRAINT logs_logtype_check CHECK (((logtype)::text = ANY ((ARRAY['Hata'::character varying, 'Uyarı'::character varying, 'Bilgilendirme'::character varying])::text[])))
);


ALTER TABLE public.logs OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 139299)
-- Name: logs_logid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.logs_logid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.logs_logid_seq OWNER TO postgres;

--
-- TOC entry 3357 (class 0 OID 0)
-- Dependencies: 215
-- Name: logs_logid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.logs_logid_seq OWNED BY public.logs.logid;


--
-- TOC entry 214 (class 1259 OID 139282)
-- Name: orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders (
    orderid integer NOT NULL,
    customerid integer NOT NULL,
    productid integer NOT NULL,
    quantity integer NOT NULL,
    totalprice numeric(10,2) NOT NULL,
    orderdate timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    orderstatus character varying(50) NOT NULL
);


ALTER TABLE public.orders OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 139281)
-- Name: orders_orderid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_orderid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.orders_orderid_seq OWNER TO postgres;

--
-- TOC entry 3358 (class 0 OID 0)
-- Dependencies: 213
-- Name: orders_orderid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_orderid_seq OWNED BY public.orders.orderid;


--
-- TOC entry 217 (class 1259 OID 139326)
-- Name: orders_orderid_seq1; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.orders ALTER COLUMN orderid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.orders_orderid_seq1
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 212 (class 1259 OID 139274)
-- Name: products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.products (
    productid integer NOT NULL,
    productname character varying(255) NOT NULL,
    stock integer NOT NULL,
    price numeric(10,2) NOT NULL,
    CONSTRAINT products_price_check CHECK ((price >= (0)::numeric))
);


ALTER TABLE public.products OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 139273)
-- Name: products_productid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.products_productid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.products_productid_seq OWNER TO postgres;

--
-- TOC entry 3359 (class 0 OID 0)
-- Dependencies: 211
-- Name: products_productid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.products_productid_seq OWNED BY public.products.productid;


--
-- TOC entry 3180 (class 2604 OID 139269)
-- Name: customers customerid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers ALTER COLUMN customerid SET DEFAULT nextval('public.customers_customerid_seq'::regclass);


--
-- TOC entry 3184 (class 2604 OID 139303)
-- Name: logs logid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs ALTER COLUMN logid SET DEFAULT nextval('public.logs_logid_seq'::regclass);


--
-- TOC entry 3182 (class 2604 OID 139277)
-- Name: products productid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products ALTER COLUMN productid SET DEFAULT nextval('public.products_productid_seq'::regclass);


--
-- TOC entry 3342 (class 0 OID 139266)
-- Dependencies: 210
-- Data for Name: customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.customers (customerid, customername, budget, customertype, totalspent) FROM stdin;
123	Customer123	2760.00	Premium	0.00
33	Customer33	505.00	Premium	400.00
76	Customer76	2343.00	Premium	0.00
77	Customer77	1606.00	Premium	0.00
78	Customer78	1164.00	Standard	0.00
3	Customer3	1518.00	Premium	0.00
4	Customer4	2058.00	Premium	0.00
6	Customer6	1133.00	Premium	0.00
7	Customer7	1196.00	Premium	0.00
8	Customer8	516.00	Standard	0.00
9	Customer9	2275.00	Premium	0.00
10	Customer10	600.00	Premium	0.00
11	Customer11	1890.00	Standard	0.00
12	Customer12	537.00	Premium	0.00
13	Customer13	1005.00	Premium	0.00
14	Customer14	2734.00	Premium	0.00
15	Customer15	1535.00	Standard	0.00
16	Customer16	2547.00	Premium	0.00
17	Customer17	1694.00	Standard	0.00
18	Customer18	2933.00	Premium	0.00
19	Customer19	639.00	Premium	0.00
20	Customer20	1246.00	Premium	0.00
21	Customer21	2608.00	Premium	0.00
81	Customer81	2877.00	Standard	0.00
82	Customer82	2898.00	Premium	0.00
26	Customer26	704.00	Premium	0.00
28	Customer28	866.00	Premium	0.00
29	Customer29	2313.00	Standard	0.00
30	Customer30	1898.00	Premium	0.00
31	Customer31	1645.00	Premium	0.00
32	Customer32	1596.00	Premium	0.00
34	Customer34	754.00	Standard	0.00
35	Customer35	1786.00	Premium	0.00
36	Customer36	2687.00	Premium	0.00
37	Customer37	2104.00	Premium	0.00
38	Customer38	1206.00	Premium	0.00
39	Customer39	2240.00	Premium	0.00
40	Customer40	1806.00	Standard	0.00
41	Customer41	677.00	Standard	0.00
42	Customer42	809.00	Premium	0.00
43	Customer43	2938.00	Premium	0.00
44	Customer44	2587.00	Premium	0.00
45	Customer45	943.00	Standard	0.00
46	Customer46	2542.00	Premium	0.00
47	Customer47	1508.00	Premium	0.00
48	Customer48	2096.00	Premium	0.00
49	Customer49	1396.00	Standard	0.00
50	Customer50	1166.00	Premium	0.00
51	Customer51	1604.00	Premium	0.00
52	Customer52	2833.00	Premium	0.00
53	Customer53	2680.00	Premium	0.00
54	Customer54	605.00	Standard	0.00
55	Customer55	1853.00	Premium	0.00
56	Customer56	821.00	Premium	0.00
57	Customer57	2303.00	Standard	0.00
5	Customer5	409.00	Standard	550.00
2	Customer2	2191.00	Premium	550.00
27	Customer27	645.00	Standard	300.00
83	Customer83	2012.00	Premium	0.00
84	Customer84	2457.00	Premium	0.00
25	Customer25	765.00	Premium	450.00
58	Customer58	783.00	Premium	0.00
59	Customer59	1489.00	Premium	0.00
60	Customer60	2297.00	Standard	0.00
61	Customer61	2794.00	Premium	0.00
62	Customer62	731.00	Premium	0.00
63	Customer63	2589.00	Standard	0.00
64	Customer64	1663.00	Premium	0.00
65	Customer65	952.00	Standard	0.00
66	Customer66	1991.00	Premium	0.00
67	Customer67	1858.00	Premium	0.00
1	Customer1	24.00	Premium	525.00
68	Customer68	2262.00	Premium	0.00
69	Customer69	1575.00	Premium	0.00
70	Customer70	2084.00	Premium	0.00
71	Customer71	2326.00	Premium	0.00
72	Customer72	2430.00	Standard	0.00
73	Customer73	2577.00	Standard	0.00
74	Customer74	517.00	Premium	0.00
75	Customer75	2321.00	Premium	0.00
79	Customer79	798.00	Premium	1215.00
124	Customer124	1572.00	Premium	0.00
80	Customer80	2377.00	Standard	300.00
85	Customer85	2013.00	Premium	0.00
86	Customer86	1911.00	Premium	0.00
87	Customer87	2359.00	Standard	0.00
88	Customer88	2165.00	Premium	0.00
89	Customer89	2650.00	Standard	0.00
90	Customer90	2833.00	Standard	0.00
91	Customer91	1654.00	Standard	0.00
92	Customer92	505.00	Premium	0.00
23	Customer23	2297.00	Standard	150.00
93	Customer93	2600.00	Premium	0.00
94	Customer94	2786.00	Premium	0.00
95	Customer95	1586.00	Premium	0.00
96	Customer96	2911.00	Premium	0.00
97	Customer97	1937.00	Premium	0.00
98	Customer98	2871.00	Standard	0.00
99	Customer99	2537.00	Premium	0.00
100	Customer100	1365.00	Premium	0.00
101	Customer101	1706.00	Premium	0.00
102	Customer102	1302.00	Standard	0.00
103	Customer103	2162.00	Premium	0.00
104	Customer104	911.00	Premium	0.00
22	Customer22	1134.00	Standard	150.00
105	Customer105	1726.00	Premium	0.00
106	Customer106	974.00	Premium	0.00
107	Customer107	1656.00	Standard	0.00
108	Customer108	1790.00	Standard	0.00
109	Customer109	520.00	Standard	0.00
24	Customer24	1622.00	Premium	150.00
110	Customer110	2551.00	Premium	0.00
111	Customer111	1608.00	Premium	0.00
112	Customer112	1604.00	Standard	0.00
113	Customer113	1576.00	Standard	0.00
114	Customer114	1418.00	Premium	0.00
115	Customer115	538.00	Standard	0.00
116	Customer116	1174.00	Premium	0.00
117	Customer117	2656.00	Premium	0.00
118	Customer118	1594.00	Premium	0.00
119	Customer119	700.00	Standard	0.00
120	Customer120	2943.00	Premium	0.00
121	Customer121	798.00	Standard	0.00
122	Customer122	2292.00	Premium	0.00
125	Customer125	1606.00	Premium	0.00
126	Customer126	554.00	Standard	0.00
127	Customer127	2161.00	Premium	0.00
128	Customer128	1934.00	Standard	0.00
129	Customer129	2030.00	Standard	0.00
130	Customer130	906.00	Premium	0.00
131	Customer131	2275.00	Premium	0.00
132	Customer132	2874.00	Premium	0.00
133	Customer133	854.00	Standard	0.00
134	Customer134	1709.00	Premium	0.00
135	Customer135	1396.00	Standard	0.00
136	Customer136	2492.00	Standard	0.00
137	Customer137	1249.00	Premium	0.00
\.


--
-- TOC entry 3348 (class 0 OID 139300)
-- Dependencies: 216
-- Data for Name: logs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.logs (logid, customerid, orderid, logdate, logtype, logdetails, customertype) FROM stdin;
1	1	251	2025-01-02 16:50:14.01131	Bilgilendirme	Satın alma isteği başarılı	Premium
2	1	252	2025-01-02 16:50:21.728766	Bilgilendirme	Satın alma isteği başarılı	Premium
3	1	253	2025-01-02 16:50:29.354257	Bilgilendirme	Satın alma isteği başarılı	Premium
4	1	254	2025-01-02 16:50:35.652965	Bilgilendirme	Satın alma isteği başarılı	Premium
5	1	255	2025-01-02 16:50:52.939769	Hata	Ürün stoğu yetersiz	Premium
6	5	256	2025-01-02 16:51:41.310193	Bilgilendirme	Satın alma isteği başarılı	Standart
7	2	257	2025-01-02 16:52:18.976826	Bilgilendirme	Satın alma isteği başarılı	Premium
8	1	251	2025-01-02 16:52:43.368465	Bilgilendirme	Satın alma başarılı	Premium
9	1	252	2025-01-02 16:52:43.374494	Hata	Bütçe yetersiz	Premium
10	1	253	2025-01-02 16:52:43.407346	Bilgilendirme	Satın alma başarılı	Premium
11	1	254	2025-01-02 16:52:43.412391	Hata	Bütçe yetersiz	Premium
13	2	257	2025-01-02 16:52:43.445345	Bilgilendirme	Satın alma başarılı	Premium
14	5	258	2025-01-02 16:55:42.276357	Hata	Ürün stoğu yetersiz	Standart
15	5	259	2025-01-02 16:56:23.952445	Hata	Ürün stoğu yetersiz	Standart
16	5	260	2025-01-02 16:58:28.74098	Bilgilendirme	Satın alma isteği başarılı	Standart
17	2	261	2025-01-02 16:59:22.402302	Bilgilendirme	Satın alma isteği başarılı	Premium
18	27	262	2025-01-02 16:59:37.758289	Bilgilendirme	Satın alma isteği başarılı	Standart
19	29	263	2025-01-02 16:59:54.334166	Bilgilendirme	Satın alma isteği başarılı	Standart
21	2	261	2025-02-16 14:50:44.820848	Bilgilendirme	Satın alma başarılı	Premium
24	25	264	2025-02-16 14:51:09.54718	Hata	Ürün stoğu yetersiz	Premium
25	25	265	2025-02-16 14:51:13.775187	Bilgilendirme	Satın alma isteği başarılı	Premium
26	25	266	2025-02-16 14:51:22.889869	Bilgilendirme	Satın alma isteği başarılı	Premium
27	25	267	2025-02-16 14:51:31.868966	Bilgilendirme	Satın alma isteği başarılı	Premium
28	25	265	2025-02-16 14:51:46.966775	Bilgilendirme	Satın alma başarılı	Premium
29	25	266	2025-02-16 14:51:46.987806	Bilgilendirme	Satın alma başarılı	Premium
30	25	267	2025-02-16 14:51:47.002772	Bilgilendirme	Satın alma başarılı	Premium
31	1	268	2025-02-16 14:54:04.918146	Hata	Bütçe yetersiz	Premium
32	1	269	2025-02-16 14:54:08.9447	Hata	Bütçe yetersiz	Premium
33	1	270	2025-02-16 14:54:22.14995	Bilgilendirme	Satın alma isteği başarılı	Premium
34	1	271	2025-02-16 14:54:29.131114	Hata	Bütçe yetersiz	Premium
35	1	272	2025-02-16 14:54:33.985885	Bilgilendirme	Satın alma isteği başarılı	Premium
36	1	273	2025-02-16 14:54:41.618493	Bilgilendirme	Satın alma isteği başarılı	Premium
37	1	274	2025-02-16 14:54:46.879883	Bilgilendirme	Satın alma isteği başarılı	Premium
38	1	275	2025-02-16 14:54:52.620131	Bilgilendirme	Satın alma isteği başarılı	Premium
39	1	270	2025-02-16 14:55:10.705353	Bilgilendirme	Satın alma başarılı	Premium
40	1	272	2025-02-16 14:55:10.711416	Hata	Bütçe yetersiz	Premium
41	1	273	2025-02-16 14:55:10.717416	Hata	Bütçe yetersiz	Premium
42	1	274	2025-02-16 14:55:10.735696	Hata	Bütçe yetersiz	Premium
43	1	275	2025-02-16 14:55:10.740757	Hata	Bütçe yetersiz	Premium
44	33	276	2025-02-16 14:56:14.459603	Bilgilendirme	Satın alma isteği başarılı	Premium
45	33	277	2025-02-16 14:56:20.575931	Bilgilendirme	Satın alma isteği başarılı	Premium
46	33	278	2025-02-16 14:56:27.122837	Bilgilendirme	Satın alma isteği başarılı	Premium
47	33	276	2025-02-16 14:56:37.283522	Bilgilendirme	Satın alma başarılı	Premium
48	33	277	2025-02-16 14:56:37.291587	Bilgilendirme	Satın alma başarılı	Premium
49	33	278	2025-02-16 14:56:37.304587	Bilgilendirme	Satın alma başarılı	Premium
50	80	279	2025-02-16 15:00:26.004436	Bilgilendirme	Satın alma isteği başarılı	Standart
51	80	280	2025-02-16 15:00:31.906312	Bilgilendirme	Satın alma isteği başarılı	Standart
54	23	281	2025-02-16 15:05:51.844933	Bilgilendirme	Satın alma isteği başarılı	Standart
55	23	282	2025-02-16 15:05:57.556304	Bilgilendirme	Satın alma isteği başarılı	Standart
58	22	283	2025-02-16 15:08:37.292468	Hata	Ürün stoğu yetersiz	Standart
59	22	284	2025-02-16 15:09:02.055429	Bilgilendirme	Satın alma isteği başarılı	Standart
60	22	285	2025-02-16 15:09:08.120705	Bilgilendirme	Satın alma isteği başarılı	Standart
61	79	286	2025-02-16 15:09:23.901561	Bilgilendirme	Satın alma isteği başarılı	Premium
64	79	286	2025-02-16 15:09:33.02144	Hata	Ürün stoğu yetersiz	Premium
65	24	287	2025-02-16 15:11:52.823592	Bilgilendirme	Satın alma isteği başarılı	Premium
66	24	288	2025-02-16 15:11:59.729521	Bilgilendirme	Satın alma isteği başarılı	Premium
67	24	289	2025-02-16 15:12:08.141266	Bilgilendirme	Satın alma isteği başarılı	Premium
68	24	287	2025-02-16 15:12:16.860709	Bilgilendirme	Satın alma başarılı	Premium
69	24	288	2025-02-16 15:12:16.873662	Hata	Ürün stoğu yetersiz	Premium
70	24	289	2025-02-16 15:12:16.881668	Hata	Ürün stoğu yetersiz	Premium
71	79	290	2025-02-16 15:13:56.295172	Bilgilendirme	Satın alma isteği başarılı	Premium
72	79	291	2025-02-16 15:14:03.058631	Bilgilendirme	Satın alma isteği başarılı	Premium
73	79	292	2025-02-16 15:14:09.854527	Bilgilendirme	Satın alma isteği başarılı	Premium
74	79	293	2025-02-16 15:14:18.399493	Bilgilendirme	Satın alma isteği başarılı	Premium
75	79	294	2025-02-16 15:14:28.848937	Bilgilendirme	Satın alma isteği başarılı	Premium
76	79	290	2025-02-16 15:15:03.715558	Bilgilendirme	Satın alma başarılı	Premium
77	79	291	2025-02-16 15:15:03.727556	Bilgilendirme	Satın alma başarılı	Premium
78	79	292	2025-02-16 15:15:03.735574	Bilgilendirme	Satın alma başarılı	Premium
79	79	293	2025-02-16 15:15:03.744568	Bilgilendirme	Satın alma başarılı	Premium
80	79	294	2025-02-16 15:15:03.755568	Bilgilendirme	Satın alma başarılı	Premium
81	79	295	2025-02-16 15:15:32.043425	Bilgilendirme	Satın alma isteği başarılı	Premium
82	28	296	2025-02-16 15:15:53.246268	Bilgilendirme	Satın alma isteği başarılı	Premium
83	27	297	2025-02-16 15:16:12.451505	Bilgilendirme	Satın alma isteği başarılı	Standart
84	79	298	2025-02-16 15:16:50.011672	Bilgilendirme	Satın alma isteği başarılı	Premium
\.


--
-- TOC entry 3346 (class 0 OID 139282)
-- Dependencies: 214
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.orders (orderid, customerid, productid, quantity, totalprice, orderdate, orderstatus) FROM stdin;
255	1	5	5	2500.00	2025-01-02 16:50:46.24145+03	Hata
251	1	2	5	250.00	2025-01-02 16:50:12.906175+03	Onaylandı
252	1	1	5	500.00	2025-01-02 16:50:20.874872+03	Hata
253	1	3	5	225.00	2025-01-02 16:50:28.541906+03	Onaylandı
254	1	4	5	375.00	2025-01-02 16:50:34.596179+03	Hata
256	5	2	5	250.00	2025-01-02 16:51:39.628216+03	Onaylandı
257	2	2	5	250.00	2025-01-02 16:52:17.307363+03	Onaylandı
258	5	2	4	200.00	2025-01-02 16:55:37.821122+03	Hata
259	5	2	4	200.00	2025-01-02 16:56:05.406605+03	Hata
260	5	4	4	300.00	2025-01-02 16:58:28.057775+03	Onaylandı
261	2	1	3	300.00	2025-01-02 16:59:21.712088+03	Onaylandı
262	27	4	4	300.00	2025-01-02 16:59:36.975137+03	Onaylandı
263	29	2	5	250.00	2025-01-02 16:59:53.588465+03	Hata
264	25	2	5	250.00	2025-02-16 14:51:08.17022+03	Hata
265	25	2	4	200.00	2025-02-16 14:51:12.790363+03	Onaylandı
266	25	2	1	50.00	2025-02-16 14:51:21.453031+03	Onaylandı
267	25	2	4	200.00	2025-02-16 14:51:31.168381+03	Onaylandı
268	1	2	4	200.00	2025-02-16 14:54:03.948904+03	Hata
269	1	2	4	200.00	2025-02-16 14:54:06.855055+03	Hata
271	1	2	2	100.00	2025-02-16 14:54:27.659422+03	Hata
270	1	2	1	50.00	2025-02-16 14:54:20.93484+03	Onaylandı
272	1	2	1	50.00	2025-02-16 14:54:33.140577+03	Hata
273	1	2	1	50.00	2025-02-16 14:54:40.949861+03	Hata
274	1	2	1	50.00	2025-02-16 14:54:46.234306+03	Hata
275	1	2	1	50.00	2025-02-16 14:54:51.551629+03	Hata
276	33	2	3	150.00	2025-02-16 14:56:12.954827+03	Onaylandı
277	33	2	2	100.00	2025-02-16 14:56:19.858927+03	Onaylandı
278	33	2	3	150.00	2025-02-16 14:56:26.458955+03	Onaylandı
279	80	2	3	150.00	2025-02-16 15:00:25.189058+03	Onaylandı
280	80	2	3	150.00	2025-02-16 15:00:30.971466+03	Onaylandı
281	23	2	3	150.00	2025-02-16 15:05:51.012373+03	Onaylandı
282	23	2	3	150.00	2025-02-16 15:05:56.6975+03	Hata
283	22	2	3	150.00	2025-02-16 15:08:36.106001+03	Hata
284	22	2	3	150.00	2025-02-16 15:09:01.157019+03	Onaylandı
285	22	2	3	150.00	2025-02-16 15:09:07.406776+03	Hata
286	79	2	3	150.00	2025-02-16 15:09:23.134995+03	Hata
287	24	2	3	150.00	2025-02-16 15:11:51.393873+03	Onaylandı
288	24	2	3	150.00	2025-02-16 15:11:58.592902+03	Hata
289	24	2	3	150.00	2025-02-16 15:12:07.316652+03	Hata
290	79	3	2	90.00	2025-02-16 15:13:55.546135+03	Onaylandı
291	79	1	3	300.00	2025-02-16 15:14:02.17896+03	Onaylandı
292	79	4	3	225.00	2025-02-16 15:14:09.1415+03	Onaylandı
293	79	1	5	500.00	2025-02-16 15:14:17.779386+03	Onaylandı
294	79	1	1	100.00	2025-02-16 15:14:27.968453+03	Onaylandı
295	79	3	1	45.00	2025-02-16 15:15:31.271267+03	bekliyor
296	28	3	5	225.00	2025-02-16 15:15:52.43312+03	bekliyor
297	27	4	4	300.00	2025-02-16 15:16:11.609815+03	bekliyor
298	79	4	4	300.00	2025-02-16 15:16:48.958781+03	bekliyor
\.


--
-- TOC entry 3344 (class 0 OID 139274)
-- Dependencies: 212
-- Data for Name: products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.products (productid, productname, stock, price) FROM stdin;
5	Product5	0	500.00
2	Product2	0	50.00
3	Product3	184	45.00
4	Product4	56	75.00
1	Product1	485	100.00
\.


--
-- TOC entry 3360 (class 0 OID 0)
-- Dependencies: 209
-- Name: customers_customerid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customers_customerid_seq', 137, true);


--
-- TOC entry 3361 (class 0 OID 0)
-- Dependencies: 215
-- Name: logs_logid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.logs_logid_seq', 84, true);


--
-- TOC entry 3362 (class 0 OID 0)
-- Dependencies: 213
-- Name: orders_orderid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_orderid_seq', 1, false);


--
-- TOC entry 3363 (class 0 OID 0)
-- Dependencies: 217
-- Name: orders_orderid_seq1; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_orderid_seq1', 298, true);


--
-- TOC entry 3364 (class 0 OID 0)
-- Dependencies: 211
-- Name: products_productid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.products_productid_seq', 35, true);


--
-- TOC entry 3191 (class 2606 OID 139272)
-- Name: customers customers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pkey PRIMARY KEY (customerid);


--
-- TOC entry 3197 (class 2606 OID 139309)
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (logid);


--
-- TOC entry 3195 (class 2606 OID 139288)
-- Name: orders orders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_pkey PRIMARY KEY (orderid);


--
-- TOC entry 3193 (class 2606 OID 139280)
-- Name: products products_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (productid);


--
-- TOC entry 3198 (class 2606 OID 139289)
-- Name: orders fk_customer; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_customer FOREIGN KEY (customerid) REFERENCES public.customers(customerid);


--
-- TOC entry 3200 (class 2606 OID 139310)
-- Name: logs fk_customer; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT fk_customer FOREIGN KEY (customerid) REFERENCES public.customers(customerid);


--
-- TOC entry 3201 (class 2606 OID 139315)
-- Name: logs fk_order; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT fk_order FOREIGN KEY (orderid) REFERENCES public.orders(orderid);


--
-- TOC entry 3199 (class 2606 OID 139294)
-- Name: orders fk_product; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_product FOREIGN KEY (productid) REFERENCES public.products(productid);


--
-- TOC entry 3355 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2025-02-16 16:51:43

--
-- PostgreSQL database dump complete
--

