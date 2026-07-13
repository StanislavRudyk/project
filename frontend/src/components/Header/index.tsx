import styles from './Header.module.css';

export const Header = () => {
    return (
        <header className={styles.header}>
            
            <div className={styles.logoArea}>
                <img src="/logo.png" alt="Логотип" className={styles.logoImage} />
                <h2 className={styles.logoText}>BookShare</h2>
            </div>

            <nav className={styles.nav}>
                <a className={styles.navLink}>Главная</a>
            </nav>

            <div className={styles.actions}>
                <button className={styles.loginButton}>Войти</button>
            </div>

        </header>
    );
};
