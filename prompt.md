# Prompt: Generate `project_proposal.docx` (Simple Black & White, Human-Written)

---

## 📌 Objective & Tone Guidelines

Write and generate a complete, professional, yet **simple black-and-white**, **human-written** project proposal document saved as **`project_proposal.docx`** for **PawHaven BD** (a pet care e-commerce and pet profile platform).

### Tone & Writing Style:
- **Human-Written & Natural:** Avoid robotic, overly dense AI buzzwords (e.g. do NOT use words like "synergy", "paradigm", "delve", "holistic panacea", "bespoke orchestrations"). Write in a clear, straightforward, practical tone—like a skilled software engineering lead or computer science student explaining their real-world project to a supervisor or client.
- **Strictly Monochrome / Black & White:** No flashy corporate colors, no bright teals, no rainbow badges, no colorful tables. The entire document should use a classic monochrome palette: pure black headings, dark charcoal text, light gray table header fills, and subtle border lines.
- **Honest Technical Status:**
  - **Frontend:** Marked as **`COMPLETE` (100%)** — All 18 pages built, mobile touch-optimized, client state managed with vanilla JS ES modules, and 1-click test logins ready.
  - **Database:** Marked as **`COMPLETE` (100%)** — 25 normalized tables, foreign keys, triggers, stored views, and seed datasets tested and ready.
  - **Backend:** Marked as **`INCOMPLETE` (Work in Progress — ~40%)** — Core ASP.NET Core CRUD controllers exist, but real production integrations (bKash/Nagad payments, SMS OTP verification, SignalR live tracking, Redis caching, cloud asset storage) are not yet implemented.

---

## 📑 Required Document Structure

The generated proposal document must include the following 10 sections:

1. **Cover / Header Block:** Document title, project name, date, version, and a clean metadata summary.
2. **1. Introduction & Background:** Real-world context of pet ownership in Bangladesh, challenges with buying genuine pet supplies, fragmented Facebook pages, and why PawHaven BD was created.
3. **2. Project Objectives:** 5 clear goals (species-focused catalog, mobile-first design, pet profile management, inventory-backed order pipeline, operations dashboard).
4. **3. Current Development Status Summary:** A clean status comparison table explicitly marking:
   - Frontend: **COMPLETE**
   - Database: **COMPLETE**
   - Backend: **INCOMPLETE**
5. **4. Completed Frontend Storefront:** Detailed breakdown of all 18 pages, touch-first ergonomics, sticky bottom navigation, slide-over mobile drawer, live autocomplete search, multi-facet filtering (`shop.html`), product details (`product.html`), cart (`cart.html`), checkout (`checkout.html`), pet profile manager (`my-pets.html`), and 1-click demo logins.
6. **5. Completed Database Architecture:** 25 normalized tables, InnoDB constraints, composite indexing, stored views (`v_active_products`, `v_inventory_status`), inventory audit triggers, and seed data.
7. **6. Incomplete Backend Implementation:** An honest explanation of what works (.NET 10 API, CRUD controllers, EF Core context, JWT tokens) and an itemized table of what remains to be built (bKash/Nagad payment gateways, SMS OTP, SignalR live order tracker, Redis caching, cloud photo storage, background jobs).
8. **7. Future Implementation Features & Roadmap:** 
   - *Phase 2 (Immediate):* Payments, courier API integration, SMS dispatch alerts.
   - *Phase 3 (3–6 Months):* Recurring kibble subscriptions (5% discount), veterinary clinic appointment booking, lost & found pet community network, pet vaccination reminders.
   - *Phase 4 (6–12 Months):* Smart pet nutrition advisor, PawPoints loyalty rewards, artisan vendor marketplace.
9. **8. Technology Stack Summary:** Clean table summarizing Frontend (HTML5/Tailwind/Vanilla JS), Backend (.NET 10 Web API), Database (MySQL 8+), Auth (JWT), and QA (Python test matrix).
10. **9. Proposed Timeline for Backend Completion:** Realistic 8-to-10-week sprint plan.
11. **10. Conclusion & Sign-Off:** Practical closing thoughts and signature/approval table.

---

## 💻 Automation Script: Direct Generation via Python

To generate `project_proposal.docx` immediately with clean monochrome typography, run the Python script below (requires `python-docx`):

```python
#!/usr/bin/env python3
"""
Generate simple black and white, human-written project proposal docx for PawHaven BD.
"""

import os
from docx import Document
from docx.shared import Inches, Pt, RGBColor
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.oxml import parse_xml
from docx.oxml.ns import nsdecls

def build_proposal():
    doc = Document()

    # Page Margins: Standard 1 inch
    for s in doc.sections:
        s.top_margin = Inches(1.0)
        s.bottom_margin = Inches(1.0)
        s.left_margin = Inches(1.0)
        s.right_margin = Inches(1.0)

        # Simple Header & Footer
        h = s.header
        hp = h.paragraphs[0]
        hp.alignment = WD_ALIGN_PARAGRAPH.RIGHT
        hrun = hp.add_run("PawHaven BD — Project Proposal & Status Report")
        hrun.font.name = "Calibri"
        hrun.font.size = Pt(8.5)
        hrun.font.color.rgb = RGBColor(120, 120, 120)

        f = s.footer
        fp = f.paragraphs[0]
        fp.alignment = WD_ALIGN_PARAGRAPH.CENTER
        frun = fp.add_run("PawHaven BD | Confidential Project Document")
        frun.font.name = "Calibri"
        frun.font.size = Pt(8.5)
        frun.font.color.rgb = RGBColor(120, 120, 120)

    # Color Constants (Monochrome)
    COLOR_BLACK = RGBColor(0, 0, 0)
    COLOR_DARK = RGBColor(35, 35, 35)
    COLOR_GRAY = RGBColor(90, 90, 90)

    def set_cell_borders(cell, top="CCCCCC", bottom="CCCCCC", left=None, right=None):
        tcPr = cell._tc.get_or_add_tcPr()
        b_xml = f'<w:tcBorders {nsdecls("w")}>'
        b_xml += f'<w:top w:val="single" w:sz="4" w:space="0" w:color="{top}"/>' if top else '<w:top w:val="none"/>'
        b_xml += f'<w:bottom w:val="single" w:sz="6" w:space="0" w:color="{bottom}"/>' if bottom else '<w:bottom w:val="none"/>'
        b_xml += f'<w:left w:val="single" w:sz="4" w:space="0" w:color="{left}"/>' if left else '<w:left w:val="none"/>'
        b_xml += f'<w:right w:val="single" w:sz="4" w:space="0" w:color="{right}"/>' if right else '<w:right w:val="none"/>'
        b_xml += '</w:tcBorders>'
        tcPr.append(parse_xml(b_xml))

    def set_cell_shading(cell, hex_color):
        s_xml = f'<w:shd {nsdecls("w")} w:fill="{hex_color}"/>'
        cell._tc.get_or_add_tcPr().append(parse_xml(s_xml))

    def set_cell_padding(cell, top=80, bottom=80, left=100, right=100):
        tcPr = cell._tc.get_or_add_tcPr()
        m_xml = f'<w:tcMar {nsdecls("w")}><w:top w:w="{top}" w:type="dxa"/><w:bottom w:w="{bottom}" w:type="dxa"/><w:left w:w="{left}" w:type="dxa"/><w:right w:w="{right}" w:type="dxa"/></w:tcMar>'
        tcPr.append(parse_xml(m_xml))

    def add_h1(text):
        p = doc.add_paragraph()
        p.paragraph_format.space_before = Pt(14)
        p.paragraph_format.space_after = Pt(4)
        p.paragraph_format.keep_with_next = True
        run = p.add_run(text)
        run.font.name = "Calibri"
        run.font.size = Pt(14)
        run.font.bold = True
        run.font.color.rgb = COLOR_BLACK
        return p

    def add_h2(text):
        p = doc.add_paragraph()
        p.paragraph_format.space_before = Pt(10)
        p.paragraph_format.space_after = Pt(3)
        p.paragraph_format.keep_with_next = True
        run = p.add_run(text)
        run.font.name = "Calibri"
        run.font.size = Pt(12)
        run.font.bold = True
        run.font.color.rgb = COLOR_DARK
        return p

    def add_p(text, bold_prefix=None, italic=False):
        p = doc.add_paragraph()
        p.paragraph_format.space_before = Pt(0)
        p.paragraph_format.space_after = Pt(4)
        p.paragraph_format.line_spacing = 1.15
        if bold_prefix:
            br = p.add_run(bold_prefix)
            br.font.name = "Calibri"
            br.font.size = Pt(10)
            br.font.bold = True
            br.font.color.rgb = COLOR_BLACK
        r = p.add_run(text)
        r.font.name = "Calibri"
        r.font.size = Pt(10)
        r.font.italic = italic
        r.font.color.rgb = COLOR_DARK
        return p

    def add_note_box(text):
        tbl = doc.add_table(rows=1, cols=1)
        tbl.alignment = WD_TABLE_ALIGNMENT.CENTER
        c = tbl.cell(0, 0)
        c.width = Inches(6.5)
        set_cell_shading(c, "F5F5F5")
        set_cell_padding(c, top=100, bottom=100, left=140, right=140)
        set_cell_borders(c, top="CCCCCC", bottom="CCCCCC", left="333333", right="CCCCCC")
        cp = c.paragraphs[0]
        cp.paragraph_format.space_before = Pt(0)
        cp.paragraph_format.space_after = Pt(0)
        r = cp.add_run(text)
        r.font.name = "Calibri"
        r.font.size = Pt(9.5)
        r.font.italic = True
        r.font.color.rgb = COLOR_DARK
        doc.add_paragraph().paragraph_format.space_after = Pt(4)

    # -------------------------------------------------------------------------
    # COVER / TITLE BLOCK
    # -------------------------------------------------------------------------
    p_title = doc.add_paragraph()
    p_title.paragraph_format.space_before = Pt(24)
    p_title.paragraph_format.space_after = Pt(2)
    t_run = p_title.add_run("PROJECT PROPOSAL & PROGRESS REPORT")
    t_run.font.name = "Calibri"
    t_run.font.size = Pt(20)
    t_run.font.bold = True
    t_run.font.color.rgb = COLOR_BLACK

    p_proj = doc.add_paragraph()
    p_proj.paragraph_format.space_before = Pt(0)
    p_proj.paragraph_format.space_after = Pt(4)
    pr_run = p_proj.add_run("PawHaven BD — Pet Care E-Commerce & Pet Management Platform")
    pr_run.font.name = "Calibri"
    pr_run.font.size = Pt(13)
    pr_run.font.bold = True
    pr_run.font.color.rgb = COLOR_DARK

    p_sub = doc.add_paragraph()
    p_sub.paragraph_format.space_before = Pt(0)
    p_sub.paragraph_format.space_after = Pt(14)
    sr = p_sub.add_run("System Overview, Completed Features, Incomplete Backend Tasks, and Future Roadmap\nDate: October 2026 | Version: 1.0")
    sr.font.name = "Calibri"
    sr.font.size = Pt(10)
    sr.font.color.rgb = COLOR_GRAY

    # Thin separator line
    p_line = doc.add_paragraph()
    p_line.paragraph_format.space_after = Pt(10)
    l_run = p_line.add_run("―" * 60)
    l_run.font.color.rgb = RGBColor(180, 180, 180)

    # -------------------------------------------------------------------------
    # SECTION 1: INTRODUCTION
    # -------------------------------------------------------------------------
    add_h1("1. Introduction & Background")
    add_p(
        "Pet ownership in Bangladesh has grown substantially over the last few years, especially in Dhaka, "
        "Chittagong, and other major urban centers. More families and young professionals are adopting cats, "
        "dogs, birds, and small pets. However, the retail ecosystem supporting these pet parents remains "
        "fragmented and unorganized."
    )
    add_p(
        "Most pet owners today have to visit scattered physical shops, place orders across informal Facebook pages, "
        "or search general multi-vendor platforms where pet items are poorly cataloged. Genuine prescription diets, "
        "litter varieties, grooming essentials, and species-appropriate supplies are frequently out of stock or "
        "difficult to verify. In addition, existing shopping websites in the country rarely account for the specific "
        "needs of pets, such as their age, breed, weight, or dietary sensitivities."
    )
    add_p(
        "PawHaven BD was started to solve these exact problems. The goal is to build a reliable, user-friendly "
        "online pet care platform tailored for pet owners in Bangladesh. It brings together species-categorized shopping, "
        "transparent inventory tracking, personalized pet profiles, and automated recommendations to make pet care simple and dependable."
    )

    # -------------------------------------------------------------------------
    # SECTION 2: PROJECT OBJECTIVES
    # -------------------------------------------------------------------------
    add_h1("2. Project Objectives")
    add_p("The key objectives of this project are:")
    add_p("Build a dedicated pet care catalog organized by species (Cats, Dogs, Birds, Rabbits, Small Pets) and subcategories, making it easy to find suitable items.", bold_prefix="1. Species-Focused Catalog: ")
    add_p("Design a mobile-first interface so that users on budget and mid-range smartphones can navigate, search, and checkout smoothly without horizontal scrolling or broken layouts.", bold_prefix="2. Mobile Accessibility: ")
    add_p("Allow pet owners to create digital profiles for their pets (age, breed, weight, medical conditions) to receive relevant product suggestions.", bold_prefix="3. Pet Profile Management: ")
    add_p("Implement an inventory-backed order pipeline where stock is deducted accurately upon order confirmation, preventing overselling.", bold_prefix="4. Reliable Order Processing: ")
    add_p("Provide administrative tools for store staff to manage orders, review catalog stock, and moderate product reviews.", bold_prefix="5. Operations Dashboard: ")

    # -------------------------------------------------------------------------
    # SECTION 3: CURRENT STATUS SUMMARY
    # -------------------------------------------------------------------------
    add_h1("3. Current Project Status Summary")
    add_p(
        "To provide a clear, realistic picture of where the project currently stands, the table below highlights "
        "the completion status of each major layer. The frontend user interface and relational database are 100% complete, "
        "while the backend API is partially developed and marked as incomplete."
    )

    tbl_status = doc.add_table(rows=4, cols=4)
    tbl_status.alignment = WD_TABLE_ALIGNMENT.CENTER
    st_headers = ["Component", "Technologies Used", "Status", "Summary of Work Done"]
    widths = [Inches(1.4), Inches(1.8), Inches(1.1), Inches(2.2)]

    for i, h_text in enumerate(st_headers):
        c = tbl_status.cell(0, i)
        c.width = widths[i]
        set_cell_shading(c, "E8E8E8")
        set_cell_padding(c, top=80, bottom=80, left=80, right=80)
        set_cell_borders(c, top="999999", bottom="333333", left=None, right=None)
        p = c.paragraphs[0]
        r = p.add_run(h_text)
        r.font.name = "Calibri"
        r.font.size = Pt(9.5)
        r.font.bold = True
        r.font.color.rgb = COLOR_BLACK

    rows = [
        ("Frontend Storefront", "HTML5, Tailwind CSS, Vanilla JS", "COMPLETE", "18+ responsive pages, mobile drawer, search autocomplete, cart, checkout UI, demo logins."),
        ("Relational Database", "MySQL 8.0+ / MariaDB 10.5+", "COMPLETE", "25 normalized tables, foreign keys, triggers, analytical views, and seed datasets."),
        ("Backend REST API", "ASP.NET Core Web API (.NET 10)", "INCOMPLETE", "Basic CRUD controllers exist; missing bKash/Nagad payments, SMS OTP, SignalR, and caching.")
    ]

    for r_idx, r_data in enumerate(rows, start=1):
        for c_idx, val in enumerate(r_data):
            c = tbl_status.cell(r_idx, c_idx)
            c.width = widths[c_idx]
            bg = "FAFAFA" if r_idx % 2 == 1 else "FFFFFF"
            set_cell_shading(c, bg)
            set_cell_padding(c, top=70, bottom=70, left=80, right=80)
            set_cell_borders(c, top="DDDDDD", bottom="DDDDDD", left=None, right=None)
            p = c.paragraphs[0]
            r = p.add_run(val)
            r.font.name = "Calibri"
            r.font.size = Pt(9)
            if c_idx == 2:
                r.font.bold = True
                r.font.color.rgb = COLOR_BLACK
            elif c_idx == 0:
                r.font.bold = True
                r.font.color.rgb = COLOR_DARK

    doc.add_paragraph().paragraph_format.space_after = Pt(6)

    add_note_box(
        "Note on Backend Status: The backend code compiles and runs locally on port 5208 with working database queries. "
        "However, we have marked it as INCOMPLETE because production-critical services (online payment processing, "
        "SMS phone verification, background jobs, and cloud asset storage) are not yet implemented."
    )

    # -------------------------------------------------------------------------
    # SECTION 4: FRONTEND IMPLEMENTATION (COMPLETE)
    # -------------------------------------------------------------------------
    add_h1("4. Completed Frontend Storefront (Status: COMPLETE)")
    add_p(
        "The frontend is completely built and functional. We intentionally used standard semantic HTML5, "
        "Tailwind CSS, and Vanilla JavaScript (ES modules) without heavy frameworks like React or Angular. "
        "This keeps page load times minimal on mobile connections and removes the need for complex build pipelines."
    )
    add_p(
        "All 18 customer-facing and operational pages are finished, responsive, and tested across both desktop browsers "
        "and mobile viewports (from small 360px screens up to large displays)."
    )

    add_h2("Detailed List of Frontend Features:")
    add_p("The entire UI is built with touch devices in mind. Touch buttons have at least 44x44px hit areas, form inputs use a 16px font to prevent unwanted iOS auto-zooming, and horizontal category ribbons swipe naturally on mobile screens.", bold_prefix="• Smartphone Touch Optimization: ")
    add_p("On mobile devices, a persistent bottom bar gives quick 1-tap access to Home, Shop, Search, Cart (with live badge counter), and Account.", bold_prefix="• Sticky Bottom Navigation: ")
    add_p("A smooth sliding drawer provides clean navigation for pet species (Cats, Dogs, Birds, Rabbits, Small Pets), order tracking, pet profiles, and support links.", bold_prefix="• Slide-Over Species Drawer: ")
    add_p("A search modal that triggers with a 300ms debounce as the user types, displaying product names, thumbnail photos, prices, and categories in real time.", bold_prefix="• Live Search with Autocomplete: ")
    add_p("The shop page includes a multi-criteria filter sidebar (which turns into a sliding sheet on mobile) supporting category filters, pet species, price ranges, in-stock toggles, life stages (Kitten/Puppy/Adult), and food types.", bold_prefix="• Multi-Facet Catalog Filtering (shop.html): ")
    add_p("Displays multi-photo galleries, interactive weight/flavor variant pickers that update pricing dynamically, stock availability warnings, and customer reviews.", bold_prefix="• Product Detail Experience (product.html): ")
    add_p("Saved locally so items remain if the user refreshes. Includes quantity adjustments, item removal, free delivery progress bar, and subtotal calculation.", bold_prefix="• Cart Management (cart.html): ")
    add_p("Supports delivery zone selection (Dhaka 80 BDT, outside Dhaka 150 BDT), coupon validation, saved customer addresses, and order placement review.", bold_prefix="• Checkout Flow (checkout.html): ")
    add_p("Allows customers to add their pets, select breed and birth date, set weight, and check off dietary allergies and health needs.", bold_prefix="• 'My Pet' Profile Hub (my-pets.html): ")
    add_p("Customer order history list with direct links to individual order timelines, delivery statuses, and breakdown of purchased items.", bold_prefix="• Order Tracking (orders.html, order-details.html): ")
    add_p("On the login screen, pre-configured buttons let testers immediately fill in Administrator, Staff, or Customer test accounts with a single click.", bold_prefix="• 1-Click Demo Login Buttons (login.html): ")

    # -------------------------------------------------------------------------
    # SECTION 5: DATABASE ARCHITECTURE (COMPLETE)
    # -------------------------------------------------------------------------
    add_h1("5. Completed Database Architecture (Status: COMPLETE)")
    add_p(
        "The relational database schema is completely designed, normalized to 3rd Normal Form (3NF), "
        "and tested. It runs on MySQL 8.0+ or MariaDB 10.5+ under the database name `pet_ecommerce_db`."
    )
    add_p(
        "The schema contains 25 normalized tables, foreign key constraints with proper cascade actions, "
        "performance indexes, pre-defined views for common queries, automated inventory audit triggers, "
        "and extensive seed data."
    )

    add_h2("Overview of Database Tables (25 Tables):")
    add_p("users (with roles 'admin', 'staff', 'customer' and BCrypt salted password hashes), customer_addresses.", bold_prefix="• User & Address Domain: ")
    add_p("pet_types (species), categories (hierarchical categories and subcategories), brands, products, product_images, product_variants (size/weight SKUs).", bold_prefix="• Catalog & Taxonomy Domain: ")
    add_p("carts, cart_items, orders, order_items, coupons, coupon_usages.", bold_prefix="• Commerce & Transactions Domain: ")
    add_p("reviews, product_questions, wishlists, wishlist_items, pet_profiles, notifications, contact_messages.", bold_prefix="• Customer Engagement Domain: ")
    add_p("inventory_transactions (audit history of stock changes), banners, blog_categories, blog_posts.", bold_prefix="• Operations & Marketing Domain: ")

    add_h2("Key Database Highlights:")
    add_p("Composite indexes were created for fast catalog filtering on (category_id, is_active), user order lookups on (user_id, created_at), and coupon code searches.", bold_prefix="• Performance Indexes: ")
    add_p("Includes 'v_active_products' (denormalized catalog view with brand and category data), 'v_inventory_status' (low-stock and out-of-stock monitor), and 'v_daily_sales_analytics'.", bold_prefix="• Stored Views: ")
    add_p("Database triggers log every inventory deduction to `inventory_transactions` whenever an order item is created, providing a full audit trail.", bold_prefix="• Inventory Triggers: ")
    add_p("Pre-loaded with 42 real pet care products, brand records, categories, promo coupons, sample user accounts, and sample orders for immediate local testing.", bold_prefix="• Complete Seed Data: ")
    add_p("We created automated scripts for both Windows (setup_database.bat and setup_database.ps1) and Linux/macOS (setup_database.sh) that create the database and seed it in one step.", bold_prefix="• 1-Click Deployment Scripts: ")

    # -------------------------------------------------------------------------
    # SECTION 6: INCOMPLETE BACKEND (WORK IN PROGRESS)
    # -------------------------------------------------------------------------
    add_h1("6. Incomplete Backend Implementation (Status: INCOMPLETE / WORK IN PROGRESS)")
    add_p(
        "The backend is built using ASP.NET Core Web API (.NET 10). It is currently in a functional "
        "prototype stage. The basic framework and endpoints are working, but it is not yet ready for "
        "production deployment because essential external integrations and enterprise features have not been added."
    )

    add_h2("What Is Currently Working in the Backend:")
    add_p("18 API controllers handling authentication, product listing, categories, brands, cart, wishlist, orders, pet profiles, coupons, and reviews.", bold_prefix="• Core REST API Controllers: ")
    add_p("Entity Framework Core mapping all 25 database tables with DbContext configuration.", bold_prefix="• ORM Mapping: ")
    add_p("JWT token generation and verification for customer and staff accounts.", bold_prefix="• Authentication: ")
    add_p("Interactive Swagger documentation available at http://localhost:5208/swagger for manual endpoint testing.", bold_prefix="• Swagger UI: ")
    add_p("Consistent response formats across all endpoints with standard HTTP status codes.", bold_prefix="• Standard Response Envelopes: ")

    add_h2("What Is Missing / Incomplete (Work to be Done):")
    add_p(
        "The following modules are incomplete or not yet implemented. Completing these items is the "
        "main technical requirement before the platform can go live:"
    )

    tbl_backend = doc.add_table(rows=7, cols=3)
    tbl_backend.alignment = WD_TABLE_ALIGNMENT.CENTER
    b_headers = ["Incomplete Module", "Current State", "What Needs to Be Built"]
    b_widths = [Inches(1.8), Inches(1.3), Inches(3.4)]

    for i, h_text in enumerate(b_headers):
        c = tbl_backend.cell(0, i)
        c.width = b_widths[i]
        set_cell_shading(c, "E8E8E8")
        set_cell_padding(c, top=80, bottom=80, left=80, right=80)
        set_cell_borders(c, top="999999", bottom="333333", left=None, right=None)
        p = c.paragraphs[0]
        r = p.add_run(h_text)
        r.font.name = "Calibri"
        r.font.size = Pt(9.5)
        r.font.bold = True
        r.font.color.rgb = COLOR_BLACK

    b_rows = [
        ("Online Payment Gateways", "Not Implemented", "Integrate bKash Checkout URL API, Nagad API, and SSLCommerz gateway with IPN webhook listeners and transaction verification."),
        ("SMS OTP Verification", "Not Implemented", "Integrate a local Bangladeshi SMS gateway (e.g. SSL Wireless or Twilio) for phone-based registration and password resets."),
        ("Live Order Tracking", "Not Implemented", "Implement an ASP.NET Core SignalR hub to push real-time order status updates (e.g. 'Out for Delivery') to the user's browser."),
        ("Redis Caching Layer", "Not Implemented", "Add distributed Redis caching for high-traffic queries like category menus, homepage banners, and product catalog searches."),
        ("Cloud File Storage", "Not Implemented", "Currently product photos and banners rely on local paths; need to connect AWS S3 or Cloudinary for reliable image uploads and WebP compression."),
        ("Background Job Worker", "Not Implemented", "Set up Hangfire or Quartz.NET to automatically cancel unpaid pending orders after 24 hours and trigger low-stock notification emails.")
    ]

    for r_idx, r_data in enumerate(b_rows, start=1):
        for c_idx, val in enumerate(r_data):
            c = tbl_backend.cell(r_idx, c_idx)
            c.width = b_widths[c_idx]
            bg = "FAFAFA" if r_idx % 2 == 1 else "FFFFFF"
            set_cell_shading(c, bg)
            set_cell_padding(c, top=70, bottom=70, left=80, right=80)
            set_cell_borders(c, top="DDDDDD", bottom="DDDDDD", left=None, right=None)
            p = c.paragraphs[0]
            r = p.add_run(val)
            r.font.name = "Calibri"
            r.font.size = Pt(9)
            if c_idx == 1:
                r.font.bold = True
                r.font.color.rgb = COLOR_DARK
            elif c_idx == 0:
                r.font.bold = True
                r.font.color.rgb = COLOR_DARK

    doc.add_paragraph().paragraph_format.space_after = Pt(6)

    # -------------------------------------------------------------------------
    # SECTION 7: FUTURE IMPLEMENTATION ROADMAP
    # -------------------------------------------------------------------------
    add_h1("7. Future Implementation Features & Roadmap")
    add_p(
        "Beyond finishing the core backend, we have mapped out future feature releases across three phases "
        "to expand the platform from a simple store into a complete pet care ecosystem in Bangladesh:"
    )

    add_h2("Phase 2: Core Backend Completion & Launch Prep (Immediate Focus)")
    add_p("Enable one-click bKash and Nagad payments alongside Cash on Delivery (COD).", bold_prefix="1. MFS Payment Integration: ")
    add_p("Connect courier APIs (such as Pathao, Steadfast, and RedX) to automatically generate consignment numbers and delivery tracking links when orders are packed.", bold_prefix="2. Courier API Automation: ")
    add_p("Send instant SMS messages to customers when their order is confirmed, dispatched, or out for delivery.", bold_prefix="3. SMS Delivery Alerts: ")
    add_p("Automated warning emails sent to store managers when product stocks fall below designated minimum safety levels.", bold_prefix="4. Inventory Reorder Alerts: ")

    add_h2("Phase 3: Expanded Pet Care Services (3 to 6 Months After Launch)")
    add_p("Allow customers to subscribe to monthly deliveries of essential items like cat food, dog kibble, and cat litter, with an automatic 5% discount on recurring orders.", bold_prefix="1. Recurring Pet Food Subscriptions: ")
    add_p("An integrated booking system connecting pet parents with verified veterinary doctors in Dhaka and Chittagong for clinic appointments or video consultations.", bold_prefix="2. Veterinary Clinic Appointments: ")
    add_p("A community-driven lost and found pet notice board where owners can post missing pet alerts with photos, last seen location, and generate printable flyers.", bold_prefix="3. Lost & Found Pet Network: ")
    add_p("Automatic SMS and email alerts reminding owners when their pet is due for annual vaccinations, deworming, or flea treatments based on their registered pet profile.", bold_prefix="4. Vaccine & Health Reminders: ")

    add_h2("Phase 4: Advanced Features & Ecosystem Scale (6 to 12 Months)")
    add_p("A guided assistant that analyzes a pet's age, breed, weight, and activity level to suggest balanced diets and identify potential food allergies.", bold_prefix="1. Smart Pet Nutrition Advisor: ")
    add_p("A gamified rewards program where customers earn 'PawPoints' on every purchase, product review, and pet profile milestone, redeemable for discounts.", bold_prefix="2. PawPoints Loyalty Program: ")
    add_p("A supplier interface allowing verified local pet bakeries, accessory makers, and importers to list and fulfill specialty pet products.", bold_prefix="3. Artisan Vendor Marketplace: ")

    # -------------------------------------------------------------------------
    # SECTION 8: TECH STACK SUMMARY
    # -------------------------------------------------------------------------
    add_h1("8. Technology Stack Summary")
    add_p("A concise overview of all tools and frameworks chosen for the platform:")

    tbl_tech = doc.add_table(rows=6, cols=3)
    tbl_tech.alignment = WD_TABLE_ALIGNMENT.CENTER
    t_headers = ["Layer", "Technology Selected", "Why It Was Chosen"]
    t_widths = [Inches(1.5), Inches(1.8), Inches(3.2)]

    for i, h_text in enumerate(t_headers):
        c = tbl_tech.cell(0, i)
        c.width = t_widths[i]
        set_cell_shading(c, "E8E8E8")
        set_cell_padding(c, top=80, bottom=80, left=80, right=80)
        set_cell_borders(c, top="999999", bottom="333333", left=None, right=None)
        p = c.paragraphs[0]
        r = p.add_run(h_text)
        r.font.name = "Calibri"
        r.font.size = Pt(9.5)
        r.font.bold = True
        r.font.color.rgb = COLOR_BLACK

    tech_data = [
        ("Frontend Storefront", "HTML5, Tailwind CSS, Vanilla JS", "Fast load times on mobile connections, zero complex build tooling, easy maintenance."),
        ("Backend Web API", "ASP.NET Core Web API (.NET 10)", "High performance, strong type safety, mature ecosystem, and clean layered architecture."),
        ("Relational Database", "MySQL 8.0+ / MariaDB 10.5+", "ACID compliance for orders and stock, foreign key constraints, and ubiquitous hosting support."),
        ("Authentication", "JWT (JSON Web Tokens)", "Stateless, secure token authentication suitable for mobile clients and web browsers."),
        ("Testing & Verification", "Python 3 Automated Matrix", "Fast operational verification covering 33 end-to-end API and touch layout test cases.")
    ]

    for r_idx, r_data in enumerate(tech_data, start=1):
        for c_idx, val in enumerate(r_data):
            c = tbl_tech.cell(r_idx, c_idx)
            c.width = t_widths[c_idx]
            bg = "FAFAFA" if r_idx % 2 == 1 else "FFFFFF"
            set_cell_shading(c, bg)
            set_cell_padding(c, top=70, bottom=70, left=80, right=80)
            set_cell_borders(c, top="DDDDDD", bottom="DDDDDD", left=None, right=None)
            p = c.paragraphs[0]
            r = p.add_run(val)
            r.font.name = "Calibri"
            r.font.size = Pt(9)
            if c_idx == 0:
                r.font.bold = True
                r.font.color.rgb = COLOR_DARK

    doc.add_paragraph().paragraph_format.space_after = Pt(6)

    # -------------------------------------------------------------------------
    # SECTION 9: TIMELINE & NEXT STEPS
    # -------------------------------------------------------------------------
    add_h1("9. Proposed Timeline for Backend Completion")
    add_p(
        "To take PawHaven BD from its current state to a live commercial launch, we estimate approximately "
        "8 to 10 weeks of focused backend development and integration work:"
    )
    add_p("Build bKash, Nagad, and SSLCommerz API connectors, set up webhook listeners, and test payment callbacks.", bold_prefix="• Weeks 1 to 3: Payment Gateway Integration — ")
    add_p("Set up local SMS gateway for mobile OTP registration and password resets; configure SignalR for live order updates.", bold_prefix="• Weeks 4 to 5: SMS & Real-Time Alerts — ")
    add_p("Configure Redis caching for product catalogs; connect AWS S3 / Cloudinary for photo uploads; configure Hangfire background tasks.", bold_prefix="• Weeks 6 to 7: Caching & Cloud Storage — ")
    add_p("Conduct end-to-end order flow testing, fix edge cases, review security settings, and prepare deployment servers.", bold_prefix="• Weeks 8 to 9: Testing & Hardening — ")
    add_p("Deploy database and backend API to staging, conduct final user testing, and launch publicly.", bold_prefix="• Week 10: Launch — ")

    # -------------------------------------------------------------------------
    # SECTION 10: CONCLUSION
    # -------------------------------------------------------------------------
    add_h1("10. Conclusion")
    add_p(
        "PawHaven BD has already achieved its two most time-consuming milestones: a fully realized, "
        "touch-optimized mobile storefront and a normalized, production-ready relational database schema. "
        "The remaining work is concentrated specifically on the backend API layer—primarily integrating external "
        "financial services, SMS messaging, and background task queues."
    )
    add_p(
        "With a focused effort to complete these remaining backend modules, PawHaven BD will be ready to launch "
        "as a dependable, modern e-commerce solution that genuinely serves the needs of pet owners in Bangladesh."
    )

    doc.add_paragraph().paragraph_format.space_after = Pt(14)

    # Signature Block
    sig_tbl = doc.add_table(rows=2, cols=2)
    sig_tbl.alignment = WD_TABLE_ALIGNMENT.CENTER
    for r in range(2):
        for c in range(2):
            cell = sig_tbl.cell(r, c)
            cell.width = Inches(3.25)
            set_cell_padding(cell, top=100, bottom=100, left=100, right=100)
            set_cell_borders(cell, top="CCCCCC", bottom="CCCCCC", left="CCCCCC", right="CCCCCC")
            set_cell_shading(cell, "F9F9F9")

    sig_tbl.cell(0, 0).paragraphs[0].add_run("Prepared by:\n\n___________________________\nProject Lead / Developer\nPawHaven BD Team").font.size = Pt(9)
    sig_tbl.cell(0, 1).paragraphs[0].add_run("Reviewed & Approved by:\n\n___________________________\nSupervisor / Stakeholder\n").font.size = Pt(9)
    sig_tbl.cell(1, 0).paragraphs[0].add_run("Date: _____________________").font.size = Pt(9)
    sig_tbl.cell(1, 1).paragraphs[0].add_run("Target Completion: Q1 2027").font.size = Pt(9)

    out = "project_proposal.docx"
    doc.save(out)
    print(f"Generated clean monochrome human-written proposal: {out} ({os.path.getsize(out)} bytes)")

if __name__ == "__main__":
    build_proposal()
```

---

## 🚀 Execution Instructions

Run the embedded script using Python:

```bash
python3 -c '
with open("prompt.md") as f:
    text = f.read()

marker = "```python\n"
start = text.find(marker) + len(marker)
end = text.find("\n```", start)
code = text[start:end]

exec(code)
'
```

The resulting **`project_proposal.docx`** will be generated with clean black-and-white typography, clear status tables, and an authentic human-written voice.