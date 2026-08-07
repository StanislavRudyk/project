import { useRef, useEffect } from 'react';
import styles from './MagneticButton.module.css';

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  strength?: number;
  children: React.ReactNode;
  variant?: 'primary' | 'accent';
}

export const MagneticButton = ({ strength = 0.35, children, variant = 'primary', className, ...rest }: Props) => {
  const ref = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    const btn = ref.current;
    if (!btn) return;

    const handleMove = (e: MouseEvent) => {
      const rect = btn.getBoundingClientRect();
      const cx = rect.left + rect.width / 2;
      const cy = rect.top + rect.height / 2;
      const dx = (e.clientX - cx) * strength;
      const dy = (e.clientY - cy) * strength;
      btn.style.transform = `translate(${dx}px, ${dy}px)`;
    };

    const handleLeave = () => {
      btn.style.transition = 'transform 0.5s cubic-bezier(0.22, 1, 0.36, 1)';
      btn.style.transform = 'translate(0, 0)';
      setTimeout(() => {
        if (btn) btn.style.transition = '';
      }, 500);
    };

    btn.addEventListener('mousemove', handleMove);
    btn.addEventListener('mouseleave', handleLeave);
    return () => {
      btn.removeEventListener('mousemove', handleMove);
      btn.removeEventListener('mouseleave', handleLeave);
    };
  }, [strength]);

  return (
    <button
      ref={ref}
      className={`${styles.btn} ${styles[variant]} ${className ?? ''}`}
      {...rest}
    >
      {children}
    </button>
  );
};
